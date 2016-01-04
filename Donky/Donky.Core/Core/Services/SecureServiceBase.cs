// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     SecureServiceBase class
//  Author:          Ben Moore
//  Created date:    03/05/2015
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
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Donky.Core.Configuration;
using Donky.Core.Exceptions;
using Donky.Core.Framework;
using Donky.Core.Framework.Logging;
using Donky.Core.Framework.Network;
using Donky.Core.Registration;

namespace Donky.Core.Services
{
	internal abstract class SecureServiceBase : ServiceBase
	{
		private readonly IServiceContext _serviceContext;
	    private readonly IRegistrationContext _registrationContext;

		protected SecureServiceBase(IRegistrationContext registrationContext, IServiceContext serviceContext, IEnvironmentInformation environmentInformation, ISdkInformation sdkInformation, IHttpClient httpClient, IConfigurationManager configurationManager) : base(registrationContext, serviceContext, environmentInformation, sdkInformation, httpClient, configurationManager)
		{
		    _registrationContext = registrationContext;
			_serviceContext = serviceContext;
		}

		protected async override Task<TResponse> ExecuteAsync<TRequest, TResponse>(string uriSuffix, HttpMethod method, TRequest request,
			bool isTokenRequestOperation = false)
		{
		    if (_registrationContext.IsSuspended)
		    {
		        throw new SuspendedException();
		    }
			if (!isTokenRequestOperation && AuthenticationRequired)
			{
				await RefreshToken();
			}

			return await base.ExecuteAsync<TRequest, TResponse>(uriSuffix, method, request, isTokenRequestOperation);
		}

		protected override async Task<bool> HandleUnauthorisedResponse(bool isTokenRequestOperation)
		{
			if (!isTokenRequestOperation)
			{
				await RefreshToken();
			}
			return true;
		}

		protected override string GetUri(string uriSuffix)
		{
			var baseUrl = _serviceContext.SecureServiceBaseUrl;
			return String.Format("{0}{1}api/{2}",
				baseUrl,
				baseUrl.EndsWith("/") ? String.Empty : "/",
				uriSuffix);
		}

		protected override Dictionary<string, string> GetHeaders(bool isTokenRequestOperation)
		{
			var headers = base.GetHeaders(isTokenRequestOperation);
			if (isTokenRequestOperation)
			{
				return headers;
			}

			headers.Remove("ApiKey");
			headers["Authorization"] = String.Format("{0} {1}", _serviceContext.TokenType, _serviceContext.AuthenticationToken);

			return headers;
		}

		public bool AuthenticationRequired
		{
			get
			{
				var tokenExpiryTime = _serviceContext.TokenExpiryTime;
				Logger.Instance.LogDebug("Token expires on {0}", tokenExpiryTime);
				var authenticationRequired = String.IsNullOrEmpty(_serviceContext.AuthenticationToken)
											 || tokenExpiryTime == null
				                             || (tokenExpiryTime <= DateTime.UtcNow.AddMinutes(-1));
				return authenticationRequired;
			}
		}
	}
}