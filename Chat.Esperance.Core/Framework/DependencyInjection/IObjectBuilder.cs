using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Esperance.Core.Framework.DependencyInjection
{
    /// <summary>
    /// Provides access to build objects using the dependency injection framework.
    /// </summary>
    internal interface IObjectBuilder : IObjectRegistry
    {
        T BuildObject<T>(string name = null) where T : class;
    }
}
