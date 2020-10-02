using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCrm.Web.Filters
{
    public class ModelStateValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                // todo: can we handle this without an if statement in each action method? 
                //context.Result = new ViewResult()
                //{
                //    ViewName = context.
                //};
                //return;
            }

            await next();
        }
    }
}
