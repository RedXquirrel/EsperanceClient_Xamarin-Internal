using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Esperance.Core.Models
{
    public class GetTokenResponseModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string as_client_id { get; set; }
        public string userName { get; set; }
        public string _issued { get; set; }
        public string _expires { get; set; }
    }
}
