using AutoMapper;
using MiniCrm.Persistence.Customer.MapperProfiles;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MiniCrm.Persistence.Tests.Customer.MapperProfiles
{

    public class AddCustomerProfileTests
    {
        [Fact]
        public void Profile_Maps_AllProperties()
        {
            var config = new MapperConfiguration(c => c.AddProfile<AddCustomerProfile>());
            config.AssertConfigurationIsValid();
        }
    }
}
