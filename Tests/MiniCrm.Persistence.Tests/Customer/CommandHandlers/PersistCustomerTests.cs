using Microsoft.EntityFrameworkCore;
using MiniCrm.DataModel;
using MiniCrm.Persistence.Customer.CommandHandlers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.EntityFrameworkCore.InMemory;
using MiniCrm.Application.Customer.Commands;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using AutoMapper;
using MiniCrm.Persistence.Customer.MapperProfiles;
using System.Linq;
using Moq;

namespace MiniCrm.Persistence.Tests.Customer.CommandHandlers
{
    public class PersistCustomerTests
    {
        [Fact]
        public async Task Customer_IsSavedToDbContext()
        {
            var entity = new DataModel.Customer
            {
                Name = "test customer"
            };

            IRequestHandler<AddCustomer, Unit> systemUnderTest = CreateSystemUnderTest(entity, out var context);

            var request = new AddCustomer
            {
                Name = "test customer"
            };

            await systemUnderTest.Handle(request, CancellationToken.None);

            Assert.Contains(context.Customers, c => c.Name == "test customer");
        }

        private PersistCustomer CreateSystemUnderTest(DataModel.Customer entity, out CrmContext context)
        {
            context = new CrmContext(
                    new DbContextOptionsBuilder<CrmContext>()
                    .UseInMemoryDatabase("db")
                    .Options);

            var mapper = new Mock<IMapper>();
            mapper.Setup(s => s.Map<DataModel.Customer>(It.IsAny<AddCustomer>())).Returns(entity);

             return new PersistCustomer(context, mapper.Object);
        }
    }
}
