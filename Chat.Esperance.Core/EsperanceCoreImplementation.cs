using Chat.Esperance.Core.Framework.DependencyInjection;
using Chat.Esperance.Core.Framework.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Chat.Esperance.Core
{
    internal class EsperanceCoreImplementation : IEsperanceCore
    {
        private readonly IRegistrationManager _registrationManager;

        internal EsperanceCoreImplementation(IRegistrationManager registrationManager, IObjectBuilder builder)
        {
            _registrationManager = registrationManager;
        }
    }
}
