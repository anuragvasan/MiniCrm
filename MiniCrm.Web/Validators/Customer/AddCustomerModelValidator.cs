using FluentValidation;
using MiniCrm.Application.Customer.Commands;
using MiniCrm.Web.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCrm.Web.Validators.Customer
{
    public class AddCustomerModelValidator : AbstractValidator<AddCustomerModel>
    {
        public AddCustomerModelValidator(AbstractValidator<AddCustomer> addCustomerValidator)
        {
            RuleFor(m => m.Customer).SetValidator(addCustomerValidator);
        }
    }
}
