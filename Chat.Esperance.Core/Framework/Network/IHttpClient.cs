using Chat.Esperance.Core.Framework.Network.Donky.Core.Framework.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Esperance.Core.Framework.Network
{
    /// <summary>
    /// Interface for general HTTP operations.
    /// </summary>
    public interface IHttpClient
    {
        /// <summary>
        /// Sends a JSON based request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        Task<HttpResponse<TResponse>> SendJsonAsync<TRequest, TResponse>(HttpRequest<TRequest> request);

        /// <summary>
        /// Downloads a stream from the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        Task<HttpResponse<Stream>> GetStreamAsync(string uri);
    }
}
