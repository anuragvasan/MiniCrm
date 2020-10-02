using FluentValidation;
using MiniCrm.Application.Customer.Commands;
using MiniCrm.Application.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCrm.Application.Customer.CommandValidators
{
    /// <summary>
    /// Defines basic validation rules for adding a customer.  The Address and PhoneNumber
    /// are validated specifically in the context of a Customer, in case these types might 
    /// have different rules in other contexts in the future.
    /// </summary>
    public class AddCustomerValidator : AbstractValidator<AddCustomer>
    {
        public AddCustomerValidator(AbstractValidator<Address> addressValidator, AbstractValidator<PhoneNumber> phoneValidator)
        {
            // note: Fluent validation cannot obtain child validators from DI directly
            // (eg via SetValidator<T> method), so we must proxy them through.
            // https://github.com/FluentValidation/FluentValidation/issues/472

            RuleFor(c => c.Name).MaximumLength(100);
            RuleFor(c => c.Email).EmailAddress().MaximumLength(100);
            RuleFor(c => c.Address).NotEmpty().SetValidator(addressValidator);
            RuleFor(c => c.Phone).NotEmpty().SetValidator(phoneValidator);
        }

        public class CustomerAddressValidator : AbstractValidator<Address>
        {
            public CustomerAddressValidator()
            {
                RuleFor(a => a.Line1).MaximumLength(100);
                RuleFor(a => a.Line2).MaximumLength(100);
                RuleFor(a => a.City).MaximumLength(100);
                RuleFor(a => a.State).MaximumLength(2);
                RuleFor(a => a.PostalCode).MaximumLength(10).Matches(@"^\d{5}(-\d{4})?$");
            }
        }

        public class CustomerPhoneNumberValidator : AbstractValidator<PhoneNumber>
        {
            public CustomerPhoneNumberValidator()
            {
                RuleFor(c => c.Number).MaximumLength(20); // consider format validation
                RuleFor(c => c.Extension).MaximumLength(10).Matches(@"^\d*$");
            }
        }
    }
}
