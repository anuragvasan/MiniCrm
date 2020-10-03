using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniCrm.Application.Customer.Queries;
using MiniCrm.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniCrm.Persistence.Customer.QueryHandlers
{
    /// <summary>
    /// Performs basic "contains" search for customers via the database.
    /// </summary>
    public class CustomerSearch : IRequestHandler<SearchCustomers, IEnumerable<SearchCustomers.CustomerSearchResult>>
    {
        private readonly CrmContext context;
        private readonly IMapper mapper;

        public CustomerSearch(CrmContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SearchCustomers.CustomerSearchResult>> Handle(SearchCustomers request, CancellationToken cancellationToken)
        {
            var predicate = context.Customers.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request?.Name))
            {
                predicate = predicate.Where(c => c.Name.Contains(request.Name));
            }

            if (!string.IsNullOrWhiteSpace(request?.Email))
            {
                predicate = predicate.Where(c => c.Email.Contains(request.Email));
            }

            // materialize the results immediately
            var results = await predicate.ToListAsync(cancellationToken);

            return results.Select(mapper.Map<SearchCustomers.CustomerSearchResult>);
        }
    }
}
