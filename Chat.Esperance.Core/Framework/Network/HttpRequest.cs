using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Esperance.Core.Framework.Network
{
    /// <summary>
    /// Represents an HTTP request with the specified type of body.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpRequest<T>
    {
        /// <summary>
        /// The target URI.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// The request body.
        /// </summary>
        public T Body { get; set; }

        /// <summary>
        /// The HTTP method to use.
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// The HTTP headers to send with the request.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }
    }
}
