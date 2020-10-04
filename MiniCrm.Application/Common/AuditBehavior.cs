using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniCrm.Application.Common
{
    /// <summary>
    /// Pipeline behavior that logs when requests are executed.  Requests that should be audited need opt-in by implementing IAuditableRequest.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class AuditBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<AuditBehavior<TRequest, TResponse>> logger;

        public AuditBehavior(ILogger<AuditBehavior<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (typeof(IAuditableRequest).IsAssignableFrom(typeof(TRequest)))
            {
                logger.LogInformation($"Executing request {typeof(TRequest).Name}: {JsonConvert.SerializeObject(request)}");
            }

            var result = await next();

            return result;
        }
    }
}
