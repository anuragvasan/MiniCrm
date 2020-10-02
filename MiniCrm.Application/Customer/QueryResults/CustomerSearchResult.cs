using MiniCrm.Application.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCrm.Application.Customer.QueryResults
{
    public class CustomerSearchResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public PhoneNumber Phone { get; set; }
        public Address Address { get; set; }
    }
}