using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCrm.Application.Common
{
    /// <summary>
    /// Marker interface indicating an IRequest should be audited before execution.
    /// </summary>
    public interface IAuditableRequest : MediatR.IBaseRequest
    {
    }
}
