using MiniCrm.Application.Customer.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MiniCrm.Application.Tests.Customer.Queries
{
    public class SearchCustomersTests
    {
        [Fact]
        public void HasAnyParameters_WhenNameGiven_True()
        {
            var query = new SearchCustomers
            {
                Name = "value"
            };

            Assert.True(query.HasAnyParameters());
        }

        [Fact]
        public void HasAnyParameters_WhenEmailGiven_True()
        {
            var query = new SearchCustomers
            {
                Email = "test@test.com",
            };

            Assert.True(query.HasAnyParameters());
        }


        [Fact]
        public void HasAnyParameters_WhenBothGiven_True()
        {
            var query = new SearchCustomers
            {
                Name = "value",
                Email = "test@test.com",
            };

            Assert.True(query.HasAnyParameters());
        }


        [Fact]
        public void HasAnyParameters_False()
        {
            var query = new SearchCustomers();

            Assert.False(query.HasAnyParameters());
        }
    }
}
