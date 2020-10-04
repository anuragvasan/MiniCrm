using Microsoft.AspNetCore.Mvc.Rendering;
using MiniCrm.Application.Customer.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCrm.Web.ViewModels.Customer
{
    public class AddCustomerModel
    {
        public AddCustomer Customer { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
    }
}
