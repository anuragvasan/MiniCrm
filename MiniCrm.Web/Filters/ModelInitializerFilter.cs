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

            var model = context.ActionArguments.ElementAt(attribute.Index).Value;

            if (model == null)
            {
                await next();
                return;
            }

            var type = model.GetType();

            var initializerType = typeof(IModelInitializer<>).MakeGenericType(type);
            var initializer = context.HttpContext.RequestServices.GetService(initializerType);

            if (initializer == null)
            {
                throw new Exception($"IModelInitializer<{type.FullName}> is not registered but referenced on {descriptor.ControllerTypeInfo.FullName}.{descriptor.MethodInfo.Name}."); // todo custom type?
            }

            if (context.ActionArguments.Count < attribute.Index)
            {
                throw new Exception($"Action argument at index {attribute.Index} is not of type {type.FullName}.  Specify 'index' paraemter for {nameof(InitializeModelAttribute)}."); 
            }
            var method = initializerType.GetMethod(nameof(IModelInitializer<object>.InitializeAsync));
            var task = (Task)method.Invoke(initializer, new[] { model, context.HttpContext.RequestAborted });
            await task;

            await next();
        }
    }
}
