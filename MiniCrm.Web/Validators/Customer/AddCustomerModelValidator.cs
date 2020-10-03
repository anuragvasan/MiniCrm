using FluentValidation;
using MiniCrm.Application.Customer.Commands;
using MiniCrm.Web.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCrm.Web.Validators.Customer
{
    /// <summary>
    /// View model validator for adding a customer.
    /// </summary>
    public class AddCustomerModelValidator : AbstractValidator<AddCustomerModel>
    {
        public AddCustomerModelValidator(AbstractValidator<AddCustomer> addCustomerValidator)
        {
            // this just delegates to the Comamnd validator itself.
            RuleFor(m => m.Customer).SetValidator(addCustomerValidator);
        }
    }
}
