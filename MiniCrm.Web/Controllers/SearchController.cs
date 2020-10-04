using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Web.ViewModels.Search;

namespace MiniCrm.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class SearchController : Controller
    {
        private readonly IMediator mediator;

        public SearchController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Display the customer search form and results.
        /// Handles both GET and POST requests, meaning that searches can be linked to via query string, eg http://localhost:59778/?search.name=john
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Search(CustomerSearchModel model, CancellationToken cancellationToken)
        {
            if (!model.Search.HasAnyParameters())
            {
                return View(model);
            }

            var results = await mediator.Send(model.Search, cancellationToken);

            return View(new CustomerSearchModel
            {
                Search = model.Search,
                Results = results,
                SearchPerformed = true,
            });
        }
    }
}
