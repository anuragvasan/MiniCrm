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
        private readonly IRequestHandler<AddCustomer> customerHandler;
        private readonly IRequestHandler<GetStates, IEnumerable<State>> stateHandler;

        public CustomerController(
            IRequestHandler<SearchCustomers, IEnumerable<CustomerSearchResult>> searchHandler,
            IRequestHandler<AddCustomer> customerHandler,
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
        [InitializeModel]
        public IActionResult Add(AddCustomerModel model)
        {
            return View(model);
        }

        [HttpPost]
        [InitializeModel]
        public async Task<IActionResult> Add(AddCustomerModel model, CancellationToken cancellationToken)
        {
            await customerHandler.Handle(model.Customer, cancellationToken);

            return RedirectToAction("Search");
        }

        // todo: move elsewhere
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
