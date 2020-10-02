using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MiniCrm.Application.Location.Queries;
using MiniCrm.Application.Location.QueryResults;
using MiniCrm.Application.Customer.Commands;
using MiniCrm.Application.Customer.Queries;
using MiniCrm.Application.Customer.QueryResults;
using MiniCrm.Web.Models;
using MiniCrm.Web.Models.Customer;

namespace MiniCrm.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IRequestHandler<SearchCustomers, IEnumerable<CustomerSearchResult>> searchHandler;
        // this feels like a MediatR bug, but IRequestHandler<AddCustomer> is not automatically registered in DI: need to add the Unit (void) response type.
        private readonly IRequestHandler<AddCustomer, Unit> customerHandler;
        private readonly IRequestHandler<GetStates, IEnumerable<State>> stateHandler;

        public CustomerController(
            IRequestHandler<SearchCustomers, IEnumerable<CustomerSearchResult>> searchHandler,
            IRequestHandler<AddCustomer, Unit> customerHandler,
            IRequestHandler<GetStates, IEnumerable<State>> stateHandler)
        {
            this.searchHandler = searchHandler;
            this.customerHandler = customerHandler;
            this.stateHandler = stateHandler;
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Search(CustomerSearchModel model, CancellationToken cancellationToken)
        {
            var results = await searchHandler.Handle(model.Search, cancellationToken);

            return View(new CustomerSearchModel
            {
                Search = model.Search,
                Results = results
            });
        }

        [HttpGet]
        public async Task<IActionResult> Add(AddCustomerModel model)
        {
            await InitializeModel(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCustomerModel model, CancellationToken cancellationToken)
        {
            // note, the ModelStateValidationFilter (registered globally) would ideally take care of this.
            // however, it currently just sets the status code.
            if (!ModelState.IsValid)
            {
                return await Add(model);
            }

            await customerHandler.Handle(model.Customer, cancellationToken);

            return RedirectToAction("Search");
        }

        private async Task InitializeModel(AddCustomerModel model)
        {
            // todo: move this into a IRequestHandler or something else?
            var states = await stateHandler.Handle(new GetStates(), CancellationToken.None);
            model.States = new[] { new SelectListItem("-Select-", "") } // todo: better place to do this?
                .Union(states.Select(s => new SelectListItem(s.Name, s.Abbreviation)));
        }

        // todo: move elsewhere
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
