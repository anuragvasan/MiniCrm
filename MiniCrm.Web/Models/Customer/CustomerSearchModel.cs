using MiniCrm.Application.Customer.Queries;
using MiniCrm.Application.Customer.QueryResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCrm.Web.Models.Customer
{
    public class CustomerSearchModel
    {
        public CustomerSearchModel()
        {
            Search = new SearchCustomers();
            Results = new CustomerSearchResult[0];
        }

        public SearchCustomers Search { get; set; }
        public IEnumerable<CustomerSearchResult> Results { get; set; }
        public bool SearchPerformed { get; set; }
    }
}
