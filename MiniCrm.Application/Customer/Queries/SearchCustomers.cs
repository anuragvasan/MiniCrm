using MediatR;
using MiniCrm.Application.Customer.QueryResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCrm.Application.Customer.Queries
{
    public class SearchCustomers : IRequest<IEnumerable<CustomerSearchResult>>
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
