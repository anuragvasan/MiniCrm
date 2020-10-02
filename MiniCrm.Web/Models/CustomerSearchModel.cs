using MiniCrm.Application.Customer.Queries;
using MiniCrm.Application.Customer.QueryResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCrm.Web.Models
{
    public class CustomerSearchModel
    {
        public SearchCustomers Search { get; set; }
        public IEnumerable<CustomerSearchResult> Results { get; set; }
    }
}
