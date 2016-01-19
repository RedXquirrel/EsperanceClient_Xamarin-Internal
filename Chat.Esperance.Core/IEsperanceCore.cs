using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Esperance.Core
{
    public interface IEsperanceCore
    {
        /// <summary>
        /// Gets the registered implementation of the specified service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetService<T>() where T : class;
    }
}
