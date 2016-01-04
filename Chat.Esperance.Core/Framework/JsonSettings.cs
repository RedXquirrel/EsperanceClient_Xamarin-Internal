using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Esperance.Core.Framework
{
    /// <summary>
    /// Standard JSON.net settings used by the SDK.
    /// </summary>
    public static class JsonSettings
    {
        /// <summary>
        /// Creates the standard settings.
        /// </summary>
        /// <returns></returns>
		public static JsonSerializerSettings CreateStandardSettings()
        {
            return new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                ContractResolver = new CamelCasePropertyNamesExceptDictionaryKeysContractResolver()
            };
        }
    }
}
