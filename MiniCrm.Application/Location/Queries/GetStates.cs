using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCrm.Application.Location.Queries
{
    /// <summary>
    /// Query to get a list of states.
    /// </summary>
    public class GetStates : IRequest<IEnumerable<GetStates.State>>
    {
        // no query parameters

        /// <summary>
        /// Data representing the result of the GetStates query.
        /// </summary>
        public class State
        {
            public string Name { get; set; }
            public string Abbreviation { get; set; }
        }
    }
}
