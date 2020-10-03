using FluentValidation;
using MiniCrm.Application.Customer.Commands;
using MiniCrm.Application.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCrm.Application.Customer.CommandValidators
{
    /// <summary>
    /// Defines validation rules for adding a customer.
    /// The Address and PhoneNumber are validated specifically 
    /// in the context of a Customer, in case these types might 
    /// have different rules in other contexts.
    /// </summary>
    public class AddCustomerValidator : AbstractValidator<AddCustomer>
    {
        public AddCustomerValidator(AbstractValidator<Address> addressValidator, 
            AbstractValidator<PhoneNumber> phoneValidator)
        {
            // note: Fluent validation cannot obtain child validators from DI directly
            // (eg via SetValidator<T> method), so we must proxy them through.
            // https://github.com/FluentValidation/FluentValidation/issues/472

            RuleFor(c => c.Name).MaximumLength(100);
            RuleFor(c => c.Email).EmailAddress().MaximumLength(100);
            RuleFor(c => c.Address).NotEmpty().SetValidator(addressValidator);
            RuleFor(c => c.Phone).NotEmpty().SetValidator(phoneValidator);

            // we need to have at least one major piece of information about the customer
            RuleFor(c => c).Must(c => !string.IsNullOrWhiteSpace(c.Name)
                || !string.IsNullOrWhiteSpace(c.Email)
                || !string.IsNullOrWhiteSpace(c.Phone?.Number))
                .WithMessage("Name, Email, or Phone Number must be given.");
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

                // when any address component is given, full information is required.
                When(a => !string.IsNullOrWhiteSpace(a.Line1)
                    || !string.IsNullOrWhiteSpace(a.Line2)
                    || !string.IsNullOrWhiteSpace(a.City)
                    || !string.IsNullOrWhiteSpace(a.State)
                    || !string.IsNullOrWhiteSpace(a.PostalCode), () =>
                    {
                        RuleFor(a => a.Line1).NotEmpty();
                        RuleFor(a => a.City).NotEmpty();
                        RuleFor(a => a.State).NotEmpty();
                        RuleFor(a => a.PostalCode).NotEmpty();
                    });
            }
        }

        public class CustomerPhoneNumberValidator : AbstractValidator<PhoneNumber>
        {
            public CustomerPhoneNumberValidator()
            {
                RuleFor(c => c.Number).MaximumLength(20); // todo: consider format validation.  client-side masking?
                RuleFor(c => c.Extension).MaximumLength(10).Matches(@"^\d*$");

                // when providing an extension, the main line must be provided too.
                When(c => !string.IsNullOrWhiteSpace(c.Extension), () => {
                    RuleFor(c => c.Number).NotEmpty();
                });
            }
        }
    }
}
