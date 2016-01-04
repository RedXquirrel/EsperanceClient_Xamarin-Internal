// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     NotificationService class.
//  Author:          Ben Moore
//  Created date:    07/05/2015
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
using System.Net.Http;
using System.Threading.Tasks;
using Donky.Core.Configuration;
using Donky.Core.Framework;
using Donky.Core.Framework.Network;
using Donky.Core.Registration;

namespace Donky.Core.Services.Notification
{
	internal class NotificationService : SecureServiceBase, INotificationService
	{
		internal NotificationService(IRegistrationContext registrationContext, IServiceContext serviceContext, IEnvironmentInformation environmentInformation, ISdkInformation sdkInformation, IHttpClient httpClient, IConfigurationManager configurationManager) : base(registrationContext, serviceContext, environmentInformation, sdkInformation, httpClient, configurationManager)
		{
		}

		public async Task<SynchroniseResult> SynchroniseAsync(SynchroniseRequest request)
		{
			return await ExecuteAsync<SynchroniseRequest, SynchroniseResult>(
				"notification/synchronise", HttpMethod.Post, request);
		}

		public async Task<ServerNotification> GetNotificationAsync(string notificationId)
		{
			return await ExecuteAsync<object, ServerNotification>(
				"notification/" + notificationId, HttpMethod.Get, null);
		}
	}
}