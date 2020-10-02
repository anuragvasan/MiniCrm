using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MiniCrm.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCrm.Web
{
    public class CompositionRoot
    {
        public void Configure(IServiceCollection collection, IConfiguration configuration)
        {
            collection.AddDbContext<CrmContext>(options => options.UseSqlServer(configuration.GetConnectionString("Crm")));
        }
    }
}
