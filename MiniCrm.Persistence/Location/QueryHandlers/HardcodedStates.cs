using MediatR;
using MiniCrm.Application.Location.Queries;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static MiniCrm.Application.Location.Queries.GetStates;

namespace MiniCrm.Persistence.Location.QueryHandlers
{
    /// <summary>
    /// Basic hard-coded implementation of the GetStates query.
    /// </summary>
    public class HardcodedStates : IRequestHandler<GetStates, IEnumerable<State>>
    {
        public Task<IEnumerable<State>> Handle(GetStates request, CancellationToken cancellationToken)
        {
            return Task.FromResult((IEnumerable<State>)new[] {
                new State { Abbreviation = "AL", Name = "Alabama" },
                new State { Abbreviation = "AK", Name = "Alaska" },
                new State { Abbreviation = "AZ", Name = "Arizona" },
                new State { Abbreviation = "AR", Name = "Arkansas" },
                new State { Abbreviation = "CA", Name = "California" },
                new State { Abbreviation = "CO", Name = "Colorado" },
                new State { Abbreviation = "CT", Name = "Connecticut" },
                new State { Abbreviation = "DE", Name = "Delaware" },
                new State { Abbreviation = "FL", Name = "Florida" },
                new State { Abbreviation = "GA", Name = "Georgia" },
                new State { Abbreviation = "HI", Name = "Hawaii" },
                new State { Abbreviation = "ID", Name = "Idaho" },
                new State { Abbreviation = "IL", Name = "Illinois" },
                new State { Abbreviation = "IN", Name = "Indiana" },
                new State { Abbreviation = "IA", Name = "Iowa" },
                new State { Abbreviation = "KS", Name = "Kansas" },
                new State { Abbreviation = "KY", Name = "Kentucky" },
                new State { Abbreviation = "LA", Name = "Louisiana" },
                new State { Abbreviation = "ME", Name = "Maine" },
                new State { Abbreviation = "MD", Name = "Maryland" },
                new State { Abbreviation = "MA", Name = "Massachusetts" },
                new State { Abbreviation = "MI", Name = "Michigan" },
                new State { Abbreviation = "MN", Name = "Minnesota" },
                new State { Abbreviation = "MS", Name = "Mississippi" },
                new State { Abbreviation = "MO", Name = "Missouri" },
                new State { Abbreviation = "MT", Name = "Montana" },
                new State { Abbreviation = "NC", Name = "North Carolina" },
                new State { Abbreviation = "ND", Name = "North Dakota" },
                new State { Abbreviation = "NE", Name = "Nebraska" },
                new State { Abbreviation = "NV", Name = "Nevada" },
                new State { Abbreviation = "NH", Name = "New Hampshire" },
                new State { Abbreviation = "NJ", Name = "New Jersey" },
                new State { Abbreviation = "NM", Name = "New Mexico" },
                new State { Abbreviation = "NY", Name = "New York" },
                new State { Abbreviation = "OH", Name = "Ohio" },
                new State { Abbreviation = "OK", Name = "Oklahoma" },
                new State { Abbreviation = "OR", Name = "Oregon" },
                new State { Abbreviation = "PA", Name = "Pennsylvania" },
                new State { Abbreviation = "RI", Name = "Rhode Island" },
                new State { Abbreviation = "SC", Name = "South Carolina" },
                new State { Abbreviation = "SD", Name = "South Dakota" },
                new State { Abbreviation = "TN", Name = "Tennessee" },
                new State { Abbreviation = "TX", Name = "Texas" },
                new State { Abbreviation = "UT", Name = "Utah" },
                new State { Abbreviation = "VT", Name = "Vermont" },
                new State { Abbreviation = "VA", Name = "Virginia" },
                new State { Abbreviation = "WA", Name = "Washington" },
                new State { Abbreviation = "WV", Name = "West Virginia" },
                new State { Abbreviation = "WI", Name = "Wisconsin" },
                new State { Abbreviation = "WY", Name = "Wyoming" }
            });
        }
    }
}
