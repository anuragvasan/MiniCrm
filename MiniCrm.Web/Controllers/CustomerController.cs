using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Web.ViewModels.Customer;
using MiniCrm.Web.Filters;
using MiniCrm.Web.Infrastructure;

namespace MiniCrm.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class CustomerController : Controller
    {
        private readonly IMediator mediator;

        public CustomerController(IMediator mediator)
        {
            this.mediator = mediator;
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
            await mediator.Send(model.Customer, cancellationToken);

            this.SetTemporaryMessage("Customer added successfully.");

            return RedirectToAction("Search", "Search");
        }
    }
}
