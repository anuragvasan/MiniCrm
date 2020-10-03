using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCrm.Application.Location.QueryResults
{
    /// <summary>
    /// Data representing the result of the GetStates query.
    /// </summary>
    public class State
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }
}
