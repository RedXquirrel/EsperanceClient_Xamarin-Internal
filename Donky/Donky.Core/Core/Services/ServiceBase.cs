// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ServiceBase class
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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Donky.Core.Configuration;
using Donky.Core.Exceptions;
using Donky.Core.Framework;
using Donky.Core.Framework.Extensions;
using Donky.Core.Framework.Logging;
using Donky.Core.Framework.Network;
using Donky.Core.Registration;
using Donky.Core.Services.Authentication;
using Donky.Core.Services.Registration;

namespace Donky.Core.Services
{
    internal abstract class ServiceBase
    {
        #region Retry Policy fields
        /// <summary>
        ///     The ServerUnavailablePollPeriod configuration string
        ///     pattern converted to a dictionary of [delay,number to retry].
        /// </summary>
        private static readonly Dictionary<string, string> _serverUnavailablePollPeriods = new Dictionary<string, string>();

        /// <summary>
        /// If the last entry in the _serverUnavailablePollPeriods contains a *, this will be true.
        /// </summary>
        private static bool _recycleLastAttempt;

        /// <summary>
        ///     The attempt number retry delay in milliseconds, where
        ///     the delay for a particular retry is the index
        ///     of the list minus one, for any attempt that is
        ///     greater than the list length minus one, the delay
        ///     is the last index unless _recycleLastAttempt is false.
        /// </summary>
        private static readonly List<int> _retryAttemptDelay = new List<int>();
        #endregion

        private readonly IConfigurationManager _configurationManager;
        private readonly IEnvironmentInformation _environmentInformation;
        private readonly IHttpClient _httpClient;
        private readonly IRegistrationContext _registrationContext;
        private readonly ISdkInformation _sdkInformation;
        private readonly IServiceContext _serviceContext;
        
        protected ServiceBase(IRegistrationContext registrationContext, IServiceContext serviceContext,
            IEnvironmentInformation environmentInformation, ISdkInformation sdkInformation, IHttpClient httpClient,
            IConfigurationManager configurationManager)
        {
            _registrationContext = registrationContext;
            _serviceContext = serviceContext;
            _httpClient = httpClient;
            _configurationManager = configurationManager;
            _environmentInformation = environmentInformation;
            _sdkInformation = sdkInformation;
        }

        protected virtual async Task<TResponse> ExecuteAsync<TRequest, TResponse>(string uriSuffix, HttpMethod method,
            TRequest request, bool isTokenRequestOperation = false) where TResponse : class
        {
            var retry = false;
            var attempt = 0;
            TResponse response = null;
            List<ValidationFailure> validationFailures = null;
            var uri = isTokenRequestOperation
                ? GetPublicServiceUri(uriSuffix)
                : GetUri(uriSuffix);
            var delay = default(TimeSpan);

            do
            {
                attempt++;
                if (delay != default(TimeSpan))
                {
                    Logger.Instance.LogDebug("Delaying service call for attempt {0} by {1}ms", attempt,
                        delay.TotalMilliseconds);
                    await Task.Delay(delay);
                    delay = default(TimeSpan);
                }

                try
                {
                    Logger.Instance.LogInformation("Calling service at {0} ({1}), attempt {2}", uri, method, attempt);
                    var httpRequest = new HttpRequest<TRequest>
                    {
                        Body = request,
                        Headers = GetHeaders(isTokenRequestOperation),
                        Uri = uri,
                        Method = method
                    };
                    var httpResponse = await _httpClient.SendJsonAsync<TRequest, TResponse>(httpRequest);
                    Logger.Instance.LogInformation("Got response with status code {0} from {1} ({2})",
                        httpResponse.StatusCode, uri, method);

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        if (isTokenRequestOperation)
                        {
                            if (_registrationContext.IsSuspended)
                            {
                                Logger.Instance.LogWarning("User no longer suspended.");
                                _registrationContext.IsSuspended = false;
                            }
                        }

                        response = httpResponse.Body;

                        AttemptSetRetryPolicy(response);

                        retry = false;
                    }
                    else
                    {
                        switch (httpResponse.StatusCode)
                        {
                            case HttpStatusCode.Unauthorized:
                                if (isTokenRequestOperation)
                                {
                                    // We can't get a token - attempt to re-register with existing details
                                    var manager = DonkyCore.Instance.GetService<IRegistrationManager>();
                                    retry = false;
                                    var success = await manager.ReregisterWithExistingDetailsAsync();
                                    if (success)
                                    {
                                        // We can't just retry, we need to rebuild the token request so call refresh token again 
                                        return await RefreshToken() as TResponse;
                                    }

                                    HandlePublicUnauthorisedResponse();
                                }
                                else
                                {
                                    retry = await HandleUnauthorisedResponse(isTokenRequestOperation);
                                }
                                break;

                            case HttpStatusCode.Forbidden:
                                if (isTokenRequestOperation)
                                {
                                    Logger.Instance.LogWarning("User has been suspended.");
                                    // User has been suspended
                                    _registrationContext.IsSuspended = true;
                                    throw new SuspendedException();
                                }

                                throw new InvalidOperationException(
                                    "Unexpected HTTP 403 Forbidden response from {0}".FormatUsing(httpRequest.Uri));

                            case HttpStatusCode.BadRequest:
                                LogValidationFailures(httpResponse.ValidationFailures, uri, method);
                                validationFailures = httpResponse.ValidationFailures;
                                retry = false;
                                break;

                            case HttpStatusCode.NotFound:
                                response = null;
                                retry = false;
                                break;

                            case HttpStatusCode.RequestTimeout:
                                delay = GetRetryDelay(attempt);
                                response = null;
                                retry = delay.TotalMilliseconds > 0; // n.b. delay will only be 0 if last item in _serverUnavailablePollPeriods contains a 1, _recycleLastAttempt = false, and this attempt number is greater than _serverUnavailablePollPeriods.Count - hence returning a 0 means retry = false.
                                break;

                            default:
                                throw new InvalidOperationException(
                                    "Unexpected HTTP response code of {0} from {1}".FormatUsing(
                                        httpResponse.StatusCode, httpRequest.Uri));
                        }
                    }
                }
                catch (Exception exception)
                {
                    retry = ShouldRetryForException(exception, attempt, out delay);
                    if (retry)
                    {
                        Logger.Instance.LogError(exception, "Error calling service at {0}. Retrying in {1}ms", uri,
                            delay.TotalMilliseconds);
                    }
                    else
                    {
                        throw;
                    }
                }
            } while (retry);

            if (response == null && validationFailures != null)
            {
                throw new ValidationException(validationFailures);
            }

            return response;
        }

        private TimeSpan GetRetryDelay(int attempt)
        {
            var milliseconds = 0;

            if (attempt > _retryAttemptDelay.Count - 1)
            {
                milliseconds = _recycleLastAttempt ? _retryAttemptDelay[_retryAttemptDelay.Count - 1] : 0;
            }
            else
            {
                milliseconds = _retryAttemptDelay[attempt - 1];
            }

            return new TimeSpan(0, 0, 0, 0, milliseconds);
        }

        private void AttemptSetRetryPolicy<TResponse>(TResponse response) where TResponse : class
        {
            if (_serverUnavailablePollPeriods.Count != 0) return; // already set!

            // Cannot retrieve ServerUnavailablePollPeriod setting 
            // from ConfigurationManager in case this call is made 
            // before the setting is in the ConfigurationManager,
            // hence we need to get it from a response on the way in:

            try
            {
                if (_serverUnavailablePollPeriods.Count == 0)
                {
                    GetServerUnavailablePollPeriodConfiguration(response);
                }
            }
            catch (Exception ex)
            {
                throw new ServerUnavailablePollPeriodConfigurationException(ex.Message, ex.InnerException);
            }
        }

        private void GetServerUnavailablePollPeriodConfiguration<TResponse>(TResponse response) where TResponse : class
        {
            var concreteAd = response as AccessDetail;
            var concreteRr = response as RegistrationResult;

            if (concreteAd == null && concreteRr == null) return;

            DeviceConfiguration config = null;

            if (concreteAd != null)
            {
                config = concreteAd.Configuration;
            }

            if (concreteRr != null)
            {
                config = concreteRr.AccessDetails.Configuration;
            }

            // An example of ServerUnavailablePollPeriod is "5,2|30,2|60,1|120,1|300,9|600,6|900,*"
            foreach (var pair in config.ConfigurationItems["ServerUnavailablePollPeriod"].Split('|').Select(csvpair => csvpair.Split(',')))
            {
                _serverUnavailablePollPeriods.Add(pair[0], pair[1]);
            }

            foreach (var pair in _serverUnavailablePollPeriods)
            {
                var tries = 0;
                if (pair.Value.Equals("*")) // it is only legal for a * to occur for the last item, in which case it means repeat last item forever
                {
                    tries = 1; 
                    _recycleLastAttempt = true;
                }
                else
                {
                    tries = Convert.ToInt32(pair.Value);
                    _recycleLastAttempt = false; // just here for clarity because if getting here it is already false.
                }

                var delay = Convert.ToInt32(pair.Key)*1000; // convert to milliseconds

                for (var x = 0; x < tries; x++)
                {
                    _retryAttemptDelay.Add(delay);
                }
            }
        }

        protected virtual Dictionary<string, string> GetHeaders(bool isTokenRequestOperation)
        {
            return new Dictionary<string, string>
            {
                {"ApiKey", _registrationContext.ApiKey},
                {
                    "DonkyClientSystemIdentifier",
                    "Donky{0}XamarinSdk".FormatUsing(_environmentInformation.OperatingSystem)
                }
            };
        }

        protected virtual string GetUri(string uriSuffix)
        {
            return GetPublicServiceUri(uriSuffix);
        }

        protected async Task<AccessDetail> RefreshToken()
        {
            var request = new TokenRequest
            {
                NetworkId = _registrationContext.NetworkId,
                DeviceSecret = _registrationContext.DeviceSecret,
                OperatingSystem = _environmentInformation.OperatingSystem,
                SdkVersion = _sdkInformation.CoreSdkVersion
            };

            var response = await ExecuteAsync<TokenRequest, AccessDetail>("authentication/gettoken",
                HttpMethod.Post, request, true);

            _serviceContext.UpdateFromAccessDetail(response);
            await _configurationManager.UpdateConfigurationAsync(
                response.Configuration.ConfigurationItems,
                response.Configuration.ConfigurationSets);

            Logger.Instance.LogDebug("New token expiry time: {0}", response.ExpiresOn);

            return response;
        }

        protected virtual bool ShouldRetryForException(Exception exception, int attempt, out TimeSpan delay)
        {
            if (exception is UnauthorizedAccessException)
            {
                delay = default(TimeSpan);
                return false;
            }

            // TODO: Set retry time correctly.
            delay = TimeSpan.FromMilliseconds(100);
            return attempt < 3;
        }

        protected virtual Task<bool> HandleUnauthorisedResponse(bool isTokenRequestOperation)
        {
            HandlePublicUnauthorisedResponse();
            return Task.FromResult(false);
        }

        private void LogValidationFailures(IEnumerable<ValidationFailure> validationFailures, string uri,
            HttpMethod method)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("Validation failure from service {0} {1} \r\n", method, uri);
            foreach (var failure in validationFailures)
            {
                builder.AppendFormat("\t{0} - {1}", failure.Property, failure.Details);
            }
            Logger.Instance.LogError(builder.ToString());
        }

        private static void HandlePublicUnauthorisedResponse()
        {
            // Default handling is just a basic access denied
            throw new UnauthorizedAccessException("Received a 401 from a service call - check API key.");
        }

        private string GetPublicServiceUri(string uriSuffix)
        {
            var baseUrl = _serviceContext.PublicServiceBaseUrl;
            return string.Format("{0}{1}api/{2}",
                baseUrl,
                baseUrl.EndsWith("/") ? string.Empty : "/",
                uriSuffix);
        }
    }
}