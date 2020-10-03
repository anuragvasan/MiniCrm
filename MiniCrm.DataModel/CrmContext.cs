using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCrm.DataModel
{
    public class CrmContext : DbContext
    {
        public CrmContext(DbContextOptions<CrmContext> options)
            : base(options)
        { }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // initializes the database with a sample customer.
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = new Guid("69769d59-cf07-4381-93b1-1e8c08a1aaf7"),
                    Name = "Test Customer",
                    AddressLine1 = "123 Main Street",
                    AddressLine2 = "Apt 321",
                    City = "Anywhere",
                    State = "MA",
                    Email = "test.customer@example.com",
                    PhoneExtension = "1234",
                    PhoneNumber = "(123) 123-1234",
                    PostalCode = "12345"
                });
        }
    }
}
