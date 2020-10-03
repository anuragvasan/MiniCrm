using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCrm.Web.Filters
{
    /// <summary>
    /// Marker attribute for an Action method whose Model should be initialized via the corresponding IModelInitializer<T>.
    /// Register the IModelInitializer<T> implementation via dependency injection.
    /// </summary>
    public class InitializeModelAttribute : ActionFilterAttribute
    {
        public InitializeModelAttribute(int index = 0)
        {
            this.Index = index;
        }

        public int Index { get; set; }
    }
}
