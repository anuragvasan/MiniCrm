using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MiniCrm.Web.Filters;
using MiniCrm.Web.ModelInitializer;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MiniCrm.Web.Tests.Filters
{
    public class ModelInitializeFilterTests
    {
        [Fact]
        public async Task Action_WithAttributeAndInitializerType_ModelIsInitialized()
        {
            var model = new TestModel();

            var systemUnderTest = CreateSystemUnderTest(
                model,
                nameof(TestController.Action),
                out var executingContext,
                out var executedContext, 
                out var serviceProvider);

            serviceProvider.Setup(c => c.GetService(typeof(IModelInitializer<TestModel>))).Returns(new TestModelInitializer());

            ActionExecutionDelegate next = () => Task.FromResult(executedContext);

            await systemUnderTest.OnActionExecutionAsync(executingContext, next);

            Assert.NotNull(model.Value);
        }

        [Fact]
        public async Task Action_WithoutAttrbute_ModelIsNotInitialized()
        {
            var model = new TestModel();

            var systemUnderTest = CreateSystemUnderTest(
                model,
                nameof(TestController.ActionNoAttribute),
                out var executingContext,
                out var executedContext,
                out var serviceProvider);

            serviceProvider.Setup(c => c.GetService(typeof(IModelInitializer<TestModel>))).Returns(new TestModelInitializer());

            ActionExecutionDelegate next = () => Task.FromResult(executedContext);

            await systemUnderTest.OnActionExecutionAsync(executingContext, next);

            Assert.Null(model.Value);
        }

        [Fact]
        public async Task Action_WithAttrbuteButNoInitializer_Throws()
        {
            var model = new TestModel();

            var systemUnderTest = CreateSystemUnderTest(
                model,
                nameof(TestController.Action),
                out var executingContext,
                out var executedContext,
                out var serviceProvider);

            ActionExecutionDelegate next = () => Task.FromResult(executedContext);

            var exception = await Assert.ThrowsAsync<Exception>(() => systemUnderTest.OnActionExecutionAsync(executingContext, next));
            Assert.Equal("IModelInitializer<MiniCrm.Web.Tests.Filters.ModelInitializeFilterTests+TestModel> is not registered but is referenced on MiniCrm.Web.Tests.Filters.ModelInitializeFilterTests+TestController.Action.", exception.Message);
        }

        public class TestController : Controller
        {
            [InitializeModel]
            public ActionResult Action(TestModel model)
            {
                return this.Ok();
            }

            public ActionResult ActionNoAttribute(TestModel model)
            {
                return this.Ok();
            }

            [InitializeModel]
            public ActionResult ActionNoModel()
            {
                return this.Ok();
            }
        }

        public class TestModel
        {
            public string Value { get; set; }
        }

        public class TestModelInitializer : IModelInitializer<TestModel>
        {
            public Task InitializeAsync(TestModel model, CancellationToken cancellationToken)
            {
                model.Value = "a value!";
                return Task.CompletedTask;
            }
        }

        private ModelInitializerFilter CreateSystemUnderTest(
            TestModel model,
            string actionName,
            out ActionExecutingContext executingContext, 
            out ActionExecutedContext executedContext,
            out Mock<IServiceProvider> serviceProvider)
        {
            var controller = new TestController();

            serviceProvider = new Mock<IServiceProvider>();
            var httpContext = new DefaultHttpContext();
            httpContext.RequestServices = serviceProvider.Object;

            var actionContext = new ActionContext
            {
                ActionDescriptor = new ControllerActionDescriptor()
                {
                    MethodInfo = typeof(TestController).GetMethod(actionName),
                    ControllerTypeInfo = typeof(TestController).GetTypeInfo()
                },
                HttpContext = httpContext,
                RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            };

            executingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object> { { "model", model } },
                controller);

            executedContext = new ActionExecutedContext(
                actionContext,
                new List<IFilterMetadata>(),
                controller);

            return new ModelInitializerFilter();
        }
    }
}
