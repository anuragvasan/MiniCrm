using FluentValidation;
using MiniCrm.Application.Customer.Commands;
using MiniCrm.Web.ViewModels.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCrm.Web.Validators.Customer
{
    /// <summary>
    /// View model validator for adding a customer.
    /// Note: this is automatically executed by FluentValidation during model binding.
    /// </summary>
    public class AddCustomerModelValidator : AbstractValidator<AddCustomerModel>
    {
        public AddCustomerModelValidator(AbstractValidator<AddCustomer> addCustomerValidator)
        {
            // this just delegates to the Command validator.
            RuleFor(m => m.Customer).SetValidator(addCustomerValidator);
        }
    }
}
