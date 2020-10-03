using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCrm.Web.Filters
{
    /// <summary>
    /// An ActionFilter which intercepts invalid requests and re-displays the form with error messages to the user (with a 400 response code.)
    /// 
    /// This eliminates the need for ModelState checks within each controller action:
    /// if (!ModelState.IsValid)
    /// {
    ///     return View(model);
    /// }
    /// 
    /// This implementation makes several assumptions:
    /// - All actions that can be invalid (eg form POSTs) intend to redisplay a View to the user.
    /// - The ViewModel is the first argument of the action.
    /// - The View Name matches the ActionName.
    /// This could be extended with a marker attribute to allow opt-in, overriding some of these assumptions, etc.
    /// </summary>
    public class ModelStateValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid)
            {
                await next();
                return;
            }

            var metadataProvider = (IModelMetadataProvider)context.HttpContext.RequestServices.GetService(typeof(IModelMetadataProvider));

            var viewModel = context.ActionArguments.FirstOrDefault().Value;
            var action = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;

            context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;

            context.Result = new ViewResult()
            {
                ViewName = action,
                ViewData = new ViewDataDictionary(metadataProvider, context.ModelState)
                {
                    Model = viewModel
                }
            };
        }
    }
}
