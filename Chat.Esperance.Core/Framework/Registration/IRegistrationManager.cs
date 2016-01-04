using Chat.Esperance.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Esperance.Core.Framework.Registration
{
    internal interface IRegistrationManager
    {
        Task<string> RegisterUser(EsperanceUser user);
    }
}
