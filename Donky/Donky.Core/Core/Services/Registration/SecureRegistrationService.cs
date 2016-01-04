// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     SecureRegistrationService class
//  Author:          Ben Moore
//  Created date:    06/05/2015
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
using System.Net.Http;
using System.Threading.Tasks;
using Donky.Core.Configuration;
using Donky.Core.Framework;
using Donky.Core.Framework.Network;
using Donky.Core.Registration;

namespace Donky.Core.Services.Registration
{
	internal class SecureRegistrationService : SecureServiceBase, ISecureRegistrationService
	{
		public SecureRegistrationService(IRegistrationContext registrationContext, IServiceContext serviceContext, IEnvironmentInformation environmentInformation, ISdkInformation sdkInformation, IHttpClient httpClient, IConfigurationManager configurationManager) : base(registrationContext, serviceContext, environmentInformation, sdkInformation, httpClient, configurationManager)
		{
		}

		public async Task UpdateUserAsync(User user)
		{
			await ExecuteAsync<User, object>("registration/user", HttpMethod.Put, user);
		}

		public async Task UpdateDeviceAsync(Device device)
		{
			await ExecuteAsync<Device, object>("registration/device", HttpMethod.Put, device);
		}

		public async Task UpdateRegistrationAsync(RegistrationDetail registration)
		{
			await ExecuteAsync<RegistrationDetail, object>("registration", HttpMethod.Put, registration);
		}

		public async Task UpdatePushRegistrationAsync(PushConfiguration pushConfiguration)
		{
			await ExecuteAsync<PushConfiguration, object>("registration/push", HttpMethod.Put, pushConfiguration);
		}

		public async Task<List<TagOption>> GetTagsAsync()
		{
			return await ExecuteAsync<object, List<TagOption>>("registration/user/tags", HttpMethod.Get, null);
		}

		public async Task PutTagsAsync(List<TagOption> tags)
		{
			await ExecuteAsync<object, List<TagOption>>("registration/user/tags", HttpMethod.Put, tags);
        }
	}
}