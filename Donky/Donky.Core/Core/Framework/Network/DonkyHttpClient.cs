// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DonkyHttpClient class.
//  Author:          Ben Moore
//  Created date:    29/04/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
/*
MIT LICENCE:
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE. */
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Donky.Core.Services;
using ModernHttpClient;
using Donky.Core.Services.Registration;
using System.Threading;

namespace Donky.Core.Framework.Network
{
	internal class DonkyHttpClient : IHttpClient
	{
		private readonly JsonMediaTypeFormatter _formatter;
		private readonly HttpClient _client;
        private readonly CancellationTokenSource _cts;
		private int _activeCalls;
		private readonly object _trackingLock = new object();

		internal DonkyHttpClient()
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
            catch(OperationCancelledException oex)
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