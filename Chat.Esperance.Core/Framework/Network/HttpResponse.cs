using Chat.Esperance.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Esperance.Core.Framework.Network
{
    namespace Donky.Core.Framework.Network
    {
        /// <summary>
        /// Represents an HTTP response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class HttpResponse<T>
        {
            /// <summary>
            /// The status code.
            /// </summary>
            public HttpStatusCode StatusCode { get; set; }

            /// <summary>
            /// True if the request was successful, otherwise false.
            /// </summary>
            public bool IsSuccessStatusCode { get; set; }

            /// <summary>
            /// The response body.
            /// </summary>
            public T Body { get; set; }

            /// <summary>
            /// Details of any validation failures reported by the service.
            /// </summary>
            public List<ValidationFailure> ValidationFailures { get; set; }
        }
    }
}
