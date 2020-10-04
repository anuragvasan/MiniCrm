using FluentValidation.TestHelper;
using MiniCrm.Application.Customer.Commands;
using MiniCrm.Application.Customer.CommandValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MiniCrm.Application.Tests.Customer.CommandValidators
{
    public class AddCustomerValidatorTests
    {
        [Fact]
        public void Invalid_When_NamePhoneEmailMissing()
        {
            var validator = new AddCustomerValidator(new AddCustomerValidator.CustomerAddressValidator(), new AddCustomerValidator.CustomerPhoneNumberValidator());

            var data = new AddCustomer();
            var result = validator.TestValidate(data);
            result.ShouldHaveAnyValidationError();
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Name, Email, or Phone Number must be given.");
        }

        [Fact]
        public void Invalid_When_ExtensionTooLong()
        {
            var validator = new AddCustomerValidator(new AddCustomerValidator.CustomerAddressValidator(), new AddCustomerValidator.CustomerPhoneNumberValidator());

            var data = GetValidCustomer();
            data.Phone.Extension = "123123123123123";
            var result = validator.TestValidate(data);
            result.ShouldHaveValidationErrorFor(c => c.Phone.Extension);
        }

        private AddCustomer GetValidCustomer()
        {
            return new AddCustomer
            {
                Email = "sample@example.com",
                Name = "Sample",
                Address = new ValueObjects.Address(),
                Phone = new ValueObjects.PhoneNumber(),
            };
        }
    }
}
