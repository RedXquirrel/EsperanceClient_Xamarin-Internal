using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Esperance.Core.Framework
{
    /// <summary>
    /// JSON.net contract resolver that uses camel case for everything except dictionary keys.
    /// </summary>
    public class CamelCasePropertyNamesExceptDictionaryKeysContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
        {
            var contract = base.CreateDictionaryContract(objectType);
            contract.PropertyNameResolver = propertyName => propertyName;
            return contract;
        }
    }
}
