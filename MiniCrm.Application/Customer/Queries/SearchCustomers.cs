﻿using MediatR;
using MiniCrm.Application.Customer.QueryResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCrm.Application.Customer.Queries
{
    /// <summary>
    /// Represents a query to search customers in the CRM.
    /// </summary>
    public class SearchCustomers : IRequest<IEnumerable<CustomerSearchResult>>
    {
        public string Name { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// Indicates whether any search parametered were provided by the user.
        /// </summary>
        /// <returns></returns>
        public bool HasAnyParameters() => !string.IsNullOrWhiteSpace(Name) 
            || !string.IsNullOrWhiteSpace(Email);
    }
}
