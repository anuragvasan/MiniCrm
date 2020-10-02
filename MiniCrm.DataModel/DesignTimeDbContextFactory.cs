using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MiniCrm.DataModel
{
    /// <summary>
    /// Required to place EF Migrations into a non-startup project.
    /// https://medium.com/oppr/net-core-using-entity-framework-core-in-a-separate-project-e8636f9dc9e5
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CrmContext>
    {
        public CrmContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../MiniCrm.Web/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<CrmContext>();
            var connectionString = configuration.GetConnectionString("Crm");
            builder.UseSqlServer(connectionString);
            return new CrmContext(builder.Options);
        }
    }
}
