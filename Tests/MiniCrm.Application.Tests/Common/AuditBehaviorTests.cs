using MediatR;
using Microsoft.Extensions.Logging;
using MiniCrm.Application.Common;
using MiniCrm.Application.Tests.TestInfrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MiniCrm.Application.Tests.Common
{
    public class AuditBehaviorTests
    {
        [Fact]
        public async Task AuditableRequest_IsLogged()
        {
            var systemUnderTest = CreateSystemUnderTest<SampleAuditableRequest>(out var logger);
            RequestHandlerDelegate<Unit> next = () => Unit.Task;

            await systemUnderTest.Handle(new SampleAuditableRequest(), CancellationToken.None, next);

            logger.VerifyLog(LogLevel.Information, "Executing request SampleAuditableRequest: {}", Times.Once());
        }

        [Fact]
        public async Task NonAuditableRequest_IsNotLogged()
        {
            var systemUnderTest = CreateSystemUnderTest<SampleNonAuditableRequest>(out var logger);
            RequestHandlerDelegate<Unit> next = () => Unit.Task;

            await systemUnderTest.Handle(new SampleNonAuditableRequest(), CancellationToken.None, next);

            logger.VerifyLog(LogLevel.Information, "Executing request SampleNonAuditableRequest: {}", Times.Never());
        }

        [Fact]
        public async Task PipelineProceedsAfter_Logging()
        {
            var systemUnderTest = CreateSystemUnderTest<SampleAuditableRequest>(out _);
            bool nextCalled = false;
            RequestHandlerDelegate<Unit> next = () =>
            {
                nextCalled = true;
                return Unit.Task;
            };

            await systemUnderTest.Handle(new SampleAuditableRequest(), CancellationToken.None, next);

            Assert.True(nextCalled);
        }

        private AuditBehavior<T, Unit> CreateSystemUnderTest<T>(
            out Mock<ILogger<AuditBehavior<T, Unit>>> logger)
        {
            logger = new Mock<ILogger<AuditBehavior<T, Unit>>>();
            return new AuditBehavior<T, Unit>(logger.Object);
        }

        public class SampleAuditableRequest : IRequest, IAuditableRequest
        {

        }

        public class SampleNonAuditableRequest : IRequest
        {

        }
    }
}
