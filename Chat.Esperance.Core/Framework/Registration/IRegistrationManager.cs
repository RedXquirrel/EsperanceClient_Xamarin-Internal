using Chat.Esperance.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Esperance.Core.Framework.Registration
{
    public interface IRegistrationManager
    {
        Task<string> RegisterUser(EsperanceUser user);
        Task<GetTokenResponseModel> GetToken(string tokenUri, string username, string password, string grantType, string clientId, string clientSecret);
    }
}
