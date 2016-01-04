// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     RegistrationContext class.
//  Author:          Ben Moore
//  Created date:    30/04/2015
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
using System.Threading.Tasks;
using Donky.Core.Framework.Storage;
using Donky.Core.Notifications.Remote;

namespace Donky.Core.Registration
{
	internal class RegistrationContext : StorageBackedContextBase, IRegistrationContext
	{
		private string _apiKey;
		private UserDetails _user;
		private DeviceDetails _device;
		private List<ModuleDefinition> _modules;
		private string _deviceSecret;
		private string _networkId;
		private string _appVersion;
		private RemoteChannelDetails _remoteChannelDetails;
		private bool? _isSuspended;

		internal RegistrationContext(IPersistentStorage storage) : base(storage)
		{
		}

        public string ApiKey
		{
		    get
		    {
		        return _apiKey ?? (_apiKey = GetValue("ApiKey"));
		    }
			set
			{
				_apiKey = value;
				SetValue(value, "ApiKey");
			}
		}

		public string AppVersion
		{
			get { return _appVersion ?? (_appVersion = GetValue("AppVersion")); }
			set
			{
				_appVersion = value;
				SetValue(value, "AppVersion");
			}
		}

		public async Task<UserDetails> GetUser()
		{
			return _user ?? (_user = await GetValueAsync<UserDetails>("User"));
		}

		public async Task SetUser(UserDetails user)
		{
			_user = user;
			await SetValueAsync(user, "User");
		}

		public async Task<DeviceDetails> GetDevice()
		{
			return _device ?? (_device = await GetValueAsync<DeviceDetails>("Device"));

		}

		public async Task SetDevice(DeviceDetails device)
		{
			_device = device;
			await SetValueAsync(device, "Device");
		}

		public async Task<List<ModuleDefinition>> GetModules()
		{
			return _modules ?? (_modules = await GetValueAsync<List<ModuleDefinition>>("Modules"));
		}

		public async Task SetModules(List<ModuleDefinition> modules)
		{
			_modules = modules;
			await SetValueAsync(modules, "Modules");
		}

		public string DeviceSecret
		{
			get { return _deviceSecret ?? (_deviceSecret = GetValue("DeviceSecret")); }
			set
			{
				_deviceSecret = value; 
				SetValue(value, "DeviceSecret");
			}
		}

		public string NetworkId
		{
			get { return _networkId ?? (_networkId = GetValue("NetworkId")); }
			set
			{
				_networkId = value; 
				SetValue(value, "NetworkId");
			}
		}

		public async Task<RemoteChannelDetails> GetRemoteChannelDetails()
		{
			return _remoteChannelDetails ?? (_remoteChannelDetails = await GetValueAsync<RemoteChannelDetails>("RemoteChannelDetails"));
		}

		public async Task SetRemoteChannelDetails(RemoteChannelDetails details)
		{
			_remoteChannelDetails = details;
			await SetValueAsync(details, "RemoteChannelDetails");
		}

		public bool IsSuspended
		{
		    get
		    {
		        return _isSuspended ?? (_isSuspended = GetBooleanValue("IsSuspended")).Value;
		    }
			set
			{
				_isSuspended = value;
				SetValue(value, "IsSuspended");
			}
		}

		protected override string CreateId(string storageItem)
		{
			return "RegistrationContext_" + storageItem;
		}
	}
}