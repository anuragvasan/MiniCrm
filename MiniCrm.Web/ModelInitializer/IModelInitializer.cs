using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniCrm.Web.ModelInitializer
{
    /// <summary>
    /// A model initializer is responsible for populating a View Model with non-user input data, eg dropdown options.
    /// </summary>
    /// <typeparam name="T">The type of the View Model to initialize.</typeparam>
    public interface IModelInitializer<T>
    {
        Task InitializeAsync(T model, CancellationToken cancellationToken);
    }
}
