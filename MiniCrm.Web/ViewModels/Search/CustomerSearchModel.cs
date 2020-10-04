using MiniCrm.Application.Customer.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCrm.Web.ViewModels.Search
{
    public class CustomerSearchModel
    {
        public CustomerSearchModel()
        {
            Search = new SearchCustomers();
            Results = new SearchCustomers.CustomerSearchResult[0];
        }

        public SearchCustomers Search { get; set; }
        public IEnumerable<SearchCustomers.CustomerSearchResult> Results { get; set; }
        public bool SearchPerformed { get; set; }
    }
}
