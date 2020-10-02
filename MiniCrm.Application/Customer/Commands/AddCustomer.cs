using MediatR;
using MiniCrm.Application.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCrm.Application.Customer.Commands
{
    public class AddCustomer : IRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public PhoneNumber Phone { get; set; }
        public Address Address { get; set; }
    }
}
