using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Esperance.Core.Models;

namespace Chat.Esperance.Core.Framework.Registration
{
    internal class RegistrationManager : IRegistrationManager
    {
        public Task<string> RegisterUser(EsperanceUser user)
        {
            return new Task<string>(() => { return "success"; });
        }

        public Task<GetTokenResponseModel> GetToken(string tokenUri, string username, string password, string grantType, string clientId, string clientSecret)
        {
            return new Task<GetTokenResponseModel>(() => { return new GetTokenResponseModel(); });
        }
    }
}
