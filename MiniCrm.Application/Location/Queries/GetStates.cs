using MediatR;
using MiniCrm.Application.Location.QueryResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCrm.Application.Location.Queries
{
    public class GetStates : IRequest<IEnumerable<State>>
    {
        // no query parameters
    }
}
