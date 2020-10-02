using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniCrm.Application.Customer.Commands;
using MiniCrm.Application.Customer.Queries;
using MiniCrm.Application.Customer.QueryResults;
using MiniCrm.Web.Models;

namespace MiniCrm.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IRequestHandler<SearchCustomers, IEnumerable<CustomerSearchResult>> searchHandler;
        private readonly IRequestHandler<AddCustomer> customerHandler;

        public CustomerController(
            IRequestHandler<SearchCustomers, IEnumerable<CustomerSearchResult>> searchHandler,
            IRequestHandler<AddCustomer> customerHandler)
        {
            this.searchHandler = searchHandler;
            this.customerHandler = customerHandler;
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
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCustomer model, CancellationToken cancellationToken)
        {
            await customerHandler.Handle(model, cancellationToken);

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
