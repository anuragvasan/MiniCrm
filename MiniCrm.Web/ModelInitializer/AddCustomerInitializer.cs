using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniCrm.Application.Location.Queries;
using MiniCrm.Web.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniCrm.Web.ModelInitializer
{
    /// <summary>
    /// Model initializer for AddCustomerModel that populates the list of state dropdown selections.
    /// </summary>
    public class AddCustomerModelInitializer : IModelInitializer<AddCustomerModel>
    {
        private readonly IRequestHandler<GetStates, IEnumerable<GetStates.State>> stateHandler;

        public AddCustomerModelInitializer(IRequestHandler<GetStates, IEnumerable<GetStates.State>> stateHandler)
        {
            this.stateHandler = stateHandler;
        }

        public async Task InitializeAsync(AddCustomerModel model, CancellationToken cancellationToken)
        {
            var states = await stateHandler.Handle(new GetStates(), cancellationToken);

            model.States = new[] {
                new SelectListItem("-Select-", "")
            }.Union(states.Select(s => new SelectListItem(s.Name, s.Abbreviation)));
        }
    }
}
