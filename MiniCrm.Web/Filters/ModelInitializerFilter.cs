using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using MiniCrm.Web.Controllers;
using MiniCrm.Web.ModelInitializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCrm.Web.Filters
{
    /// <summary>
    /// Action filter which initializes models when an Action is decorated with InitializeModelAttribute.
    /// Initialization is performed before action execution (eg before the controller action method is called.)
    /// Helpful for filling non-input model properties required to render a view (eg dropdown list options.)
    /// </summary>
    public class ModelInitializerFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor descriptor))
            {
                await next();
                return;
            }

            var attribute = descriptor.MethodInfo.GetCustomAttributes(false)
                .OfType<InitializeModelAttribute>()
                .FirstOrDefault();

            if (attribute == null)
            {
                await next();
                return;
            }

            if (context.ActionArguments.Count < attribute.Index)
            {
                throw new Exception($"Index of view model on {descriptor.ControllerTypeInfo.FullName}.{descriptor.MethodInfo.Name} is not correct.");
            }

            var model = context.ActionArguments.ElementAt(attribute.Index).Value;

            if (model == null)
            {
                await next();
                return;
            }

            var modelType = model.GetType();

            var initializerType = typeof(IModelInitializer<>).MakeGenericType(modelType);

            var initializer = context.HttpContext.RequestServices.GetService(initializerType);

            if (initializer == null)
            {
                throw new Exception($"IModelInitializer<{modelType.FullName}> is not registered but is referenced on {descriptor.ControllerTypeInfo.FullName}.{descriptor.MethodInfo.Name}.");
            }
            
            var method = initializerType.GetMethod(nameof(IModelInitializer<object>.InitializeAsync));
            var task = (Task)method.Invoke(initializer, new[] { model, context.HttpContext.RequestAborted });
            await task;

            await next();
        }
    }
}
