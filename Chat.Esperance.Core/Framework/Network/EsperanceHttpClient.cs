using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Esperance.Core.Framework.Network
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading.Tasks;
    using ModernHttpClient;
    using System.Threading;
    using Services;

    namespace Donky.Core.Framework.Network
    {
        internal class EsperanceHttpClient : IHttpClient
        {
            private readonly JsonMediaTypeFormatter _formatter;
            private readonly HttpClient _client;
            private readonly CancellationTokenSource _cts;
            private int _activeCalls;
            private readonly object _trackingLock = new object();

            internal EsperanceHttpClient()
            {
                _formatter = new JsonMediaTypeFormatter
                {
                    SerializerSettings = JsonSettings.CreateStandardSettings()
                };

                _cts = new CancellationTokenSource();
                _client = new HttpClient(new NativeMessageHandler());
            }

            public async Task<HttpResponse<TResponse>> SendJsonAsync<TRequest, TResponse>(HttpRequest<TRequest> request)
            {
                // To cancel if there is a timeout
                bool isCancelled = false;
                // Default timeout 60 Seconds
                _cts.CancelAfter(60000);

                var httpRequest = new HttpRequestMessage(request.Method, request.Uri);
                if (request.Headers != null)
                {
                    foreach (var key in request.Headers.Keys)
                    {
                        httpRequest.Headers.Add(key, request.Headers[key]);
                    }
                }

                if (request.Body != null)
                {
                    httpRequest.Content = new ObjectContent<TRequest>(request.Body, _formatter);
                }

                HttpResponseMessage httpResponse = null;

                try
                {
                    //httpResponse = await _client.SendAsync(httpRequest, _cts.Token);
                    httpResponse = await _client.SendAsync(httpRequest);
                }
                catch (OperationCancelledException oex)
                {
                    isCancelled = true;
                }

                HttpResponse<TResponse> response = new HttpResponse<TResponse>();

                if (!isCancelled)
                {
                    response = new HttpResponse<TResponse>
                    {
                        StatusCode = httpResponse.StatusCode,
                        IsSuccessStatusCode = httpResponse.IsSuccessStatusCode
                    };

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        response.Body = await httpResponse.Content.ReadAsAsync<TResponse>();
                    }
                    else if (httpResponse.StatusCode == HttpStatusCode.BadRequest)
                    {
                        response.ValidationFailures = await httpResponse.Content.ReadAsAsync<List<ValidationFailure>>();
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.RequestTimeout;
                    response.IsSuccessStatusCode = false;
                }

                return response;
            }

            public async Task<HttpResponse<Stream>> GetStreamAsync(string uri)
            {
                var httpResponse = await _client.GetAsync(uri);
                return new HttpResponse<Stream>
                {
                    StatusCode = httpResponse.StatusCode,
                    IsSuccessStatusCode = httpResponse.IsSuccessStatusCode,
                    Body = httpResponse.IsSuccessStatusCode
                        ? await httpResponse.Content.ReadAsStreamAsync()
                        : null
                };
            }
        }
    }
}
