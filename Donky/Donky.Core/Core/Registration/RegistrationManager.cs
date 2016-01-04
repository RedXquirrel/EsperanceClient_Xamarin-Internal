// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     RegistrationManager class.
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Donky.Core.Configuration;
using Donky.Core.Framework;
using Donky.Core.Framework.Events;
using Donky.Core.Framework.Extensions;
using Donky.Core.Framework.Logging;
using Donky.Core.Notifications.Remote;
using Donky.Core.Services;
using Donky.Core.Services.Authentication;
using Donky.Core.Services.Registration;

namespace Donky.Core.Registration
{
    /// <summary>
    ///     Implementation of registration logic
    /// </summary>
    internal class RegistrationManager : IRegistrationManager
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly IEnvironmentInformation _environmentInformation;
        private readonly IEventBus _eventBus;
        private readonly ILogger _logger;
        private readonly IModuleManager _moduleManager;
        private readonly IPublicRegistrationService _publicRegistrationService;
        private readonly IRegistrationContext _registrationContext;
        private readonly ISdkInformation _sdkInformation;
        private readonly ISecureRegistrationService _secureRegistrationService;
        private readonly IServiceContext _serviceContext;

        private readonly SemaphoreSlim _synchroniseLock = new SemaphoreSlim(1);
        private readonly IRefreshToken _tokenRefresher;

        private string _appVersion;

        internal RegistrationManager(
            IRegistrationContext registrationContext, 
            IModuleManager moduleManager,
            IPublicRegistrationService publicRegistrationService, 
            ISdkInformation sdkInformation,
            IEnvironmentInformation environmentInformation, 
            IServiceContext serviceContext,
            ISecureRegistrationService secureRegistrationService, 
            IConfigurationManager configurationManager,
            IEventBus eventBus, 
            IRefreshToken tokenRefresher, 
            ILogger logger)
        {
            _registrationContext = registrationContext;
            _moduleManager = moduleManager;
            _publicRegistrationService = publicRegistrationService;
            _sdkInformation = sdkInformation;
            _environmentInformation = environmentInformation;
            _serviceContext = serviceContext;
            _secureRegistrationService = secureRegistrationService;
            _configurationManager = configurationManager;
            _eventBus = eventBus;
            _tokenRefresher = tokenRefresher;
            _logger = logger;
        }

        public async Task EnsureRegisteredAsync(UserDetails user, DeviceDetails device, string appVersion,
            bool isReplacement)
        {
            // store app version for ReplaceRegistration calls
            _appVersion = appVersion;

            if (await _registrationContext.GetUser() == null || isReplacement)
            {
                // New registration or ReplaceRegistration
                await RegisterAsync(user, device, appVersion);
            }

            if (isReplacement)
            {
                // Refresh token
                await _tokenRefresher.RefreshTokenAsync();
                _registrationContext.DeviceSecret = Guid.NewGuid().ToString();
                // Update user details
                await _registrationContext.SetUser(user);
                await _registrationContext.SetDevice(device);
            }

            PublishRegistrationChanged();
        }

        public async Task UpdatePushRegistrationAsync(RemoteChannelDetails details)
        {
            if (details == null)
            {
                return;
            }

            await _synchroniseLock.WaitAsync();

            var existingDetails = await _registrationContext.GetRemoteChannelDetails();
            if (existingDetails == null || existingDetails.Token != details.Token)
            {
                if (!string.IsNullOrEmpty(details.Token))
                {
                    await _secureRegistrationService.UpdatePushRegistrationAsync(await CreatePushConfiguration(details));

                    await _registrationContext.SetRemoteChannelDetails(details);
                }
            }

            _synchroniseLock.Release();
        }

        public async Task ReplaceRegistrationAsync(UserDetails user, DeviceDetails device)
        {
            await EnsureRegisteredAsync(user, device, _appVersion, true);
        }

        public async Task<bool> ReregisterWithExistingDetailsAsync()
        {
            await _synchroniseLock.WaitAsync();

            var existingDetails = await GetRegistrationDetailsAsync();

            if (existingDetails != null)
            {
                // Force a new secret to be generated
                _registrationContext.DeviceSecret = null;
                
                // We have some details, re-register
                await
                    RegisterAsync(existingDetails.UserDetails, existingDetails.DeviceDetails,
                        _registrationContext.AppVersion);

                _synchroniseLock.Release();

                return true;
            }

            _synchroniseLock.Release();

            return false;
        }

        public async Task<bool> GetIsRegisteredAsync()
        {
            var user = await _registrationContext.GetUser();
            return user != null && !string.IsNullOrEmpty(user.UserId);
        }

        public async Task<RegistrationDetails> GetRegistrationDetailsAsync()
        {
            // Make sure to clone the details here.
            return new RegistrationDetails
            {
                UserDetails = new UserDetails(await _registrationContext.GetUser()),
                DeviceDetails = new DeviceDetails(await _registrationContext.GetDevice())
            };
        }

        public async Task<ApiResult> UpdateRegistrationDetailsAsync(UserDetails user = null, DeviceDetails device = null)
        {
            return await ApiResult.ForOperationAsync(async () =>
            {
                await _synchroniseLock.WaitAsync();

                if (user == null && device == null)
                {
                    throw new ArgumentException("user or device must be specified");
                }

                var existingUser = await _registrationContext.GetUser();
                var wasAnonymous = existingUser.IsAnonymous;
                var oldUserId = existingUser.UserId;

                if (user != null && device != null)
                {
                    // Composite update
                    var isAnonymous = wasAnonymous && oldUserId == user.UserId;
                    
                    await _secureRegistrationService.UpdateRegistrationAsync(new RegistrationDetail
                    {
                        Client = CreateClientDetail(),
                        Device = await CreateServiceDevice(device),
                        User = CreateServiceUser(user)
                    });

                    await UpdateUserInContext(user, user.UserId, isAnonymous);
                    await UpdateDeviceInContext(device);
                }
                else if (user != null)
                {
                    var isAnonymous = wasAnonymous && oldUserId == user.UserId;

                    // User only update
                    await _secureRegistrationService.UpdateUserAsync(CreateServiceUser(user));

                    await UpdateUserInContext(user, user.UserId, isAnonymous);
                }
                else
                {
                    // Device only update
                    await _secureRegistrationService.UpdateDeviceAsync(await CreateServiceDevice(device));

                    await UpdateDeviceInContext(device);
                }

                PublishRegistrationChanged();

                _synchroniseLock.Release();
            }, "UpdateRegistrationDetailsAsync");
        }

        public async Task<List<TagOption>> GetTagsAsync()
        {
            if (!await GetIsRegisteredAsync())
            {
                return null;
            }

            await _synchroniseLock.WaitAsync();

            var tags = await _secureRegistrationService.GetTagsAsync();

            _synchroniseLock.Release();

            return tags.Select(t => new TagOption
            {
                IsSelected = t.IsSelected,
                Value = t.Value
            }).ToList();
        }

        public async Task<ApiResult> SetTagsAsync(List<TagOption> tags)
        {
            return await ApiResult.ForOperationAsync(async () =>
            {
                await _synchroniseLock.WaitAsync();

                if (tags == null)
                {
                    throw new ArgumentNullException("tags");
                }

                await _secureRegistrationService
                    .PutTagsAsync(tags.Select(t => new Services.Registration.TagOption
                    {
                        IsSelected = t.IsSelected,
                        Value = t.Value
                    }).ToList());

                var user = await _registrationContext.GetUser();
                user.SelectedTags = tags.Where(t => t.IsSelected).Select(t => t.Value).ToArray();

                _synchroniseLock.Release();
            }, "SetTagsAsync");
        }

        private async Task RegisterAsync(UserDetails user, DeviceDetails device, string appVersion)
        {
            await _synchroniseLock.WaitAsync();

            var isAnonymous = user == null || string.IsNullOrEmpty(user.UserId);

            _logger.LogInformation("Starting registration for user: {0}",
                isAnonymous ? "(anonymous)" : user.UserId);

            var request = new RegistrationDetail
            {
                Client = CreateClientDetail(appVersion),
                Device = await CreateServiceDevice(device),
                User = user == null
                    ? null
                    : CreateServiceUser(user)
            };

            var response = await _publicRegistrationService.RegisterAsync(request);
            await UpdateUserInContext(user, response.UserId, isAnonymous);

            await UpdateDeviceInContext(device);
            _registrationContext.NetworkId = response.NetworkId;

            _serviceContext.UpdateFromAccessDetail(response.AccessDetails);
            await _configurationManager.UpdateConfigurationAsync(
                response.AccessDetails.Configuration.ConfigurationItems,
                response.AccessDetails.Configuration.ConfigurationSets);

            _logger.LogInformation("Registered successfully.  NetworkId: {0}",
                response.NetworkId);

            _logger.LogDebug("New token expiry time: {0}", response.AccessDetails.ExpiresOn);
            _synchroniseLock.Release();
        }

        private ClientDetail CreateClientDetail(string appVersion = null)
        {
            return new ClientDetail
            {
                AppVersion = appVersion ?? _registrationContext.AppVersion,
                CurrentLocalTime = DateTime.Now,
                ModuleVersions = GetModuleVersions(),
                SdkVersion = _sdkInformation.CoreSdkVersion
            };
        }

        private async Task<Device> CreateServiceDevice(DeviceDetails device)
        {
            return new Device
            {
                AdditionalProperties = device == null ? null : device.AdditionalProperties,
                Id = _environmentInformation.DeviceId,
                Model = _environmentInformation.Model,
                Name = device == null ? null : device.DeviceName,
                OperatingSystem = _environmentInformation.OperatingSystem,
                OperatingSystemVersion = _environmentInformation.OperatingSystemVersion,
                PushConfiguration = await CreatePushConfiguration(),
                Secret = GetOrGenerateDeviceSecret(),
                Type = device == null ? null : device.Type
            };
        }

        private async Task<PushConfiguration> CreatePushConfiguration(RemoteChannelDetails remoteChannelDetails = null)
        {
            var details = remoteChannelDetails ?? await _registrationContext.GetRemoteChannelDetails();
            if (details == null)
            {
                return null;
            }

            switch (_environmentInformation.OperatingSystem)
            {
                case "Android":
                    return new GcmPushConfiguration
                    {
                        RegistrationId = details.Token
                    };
                case "iOS":
                    
                    var apns = new ApnsPushConfiguration
                    {
                        BundleId = this._environmentInformation.AppIdentifier,
                        Token = details.Token,
                        MessageAlertSound = NotificationSoundFilename
                    };

                    return apns;
                default:
                    throw new NotSupportedException(
                        "Invalid operating system {0}".FormatUsing(_environmentInformation.OperatingSystem));
            }
        }

        private async Task UpdateDeviceInContext(DeviceDetails device)
        {
            var deviceContext = device ?? new DeviceDetails();
            deviceContext.DeviceId = _environmentInformation.DeviceId;
            deviceContext.OperatingSystem = _environmentInformation.OperatingSystem;
            deviceContext.OperatingSystemVersion = _environmentInformation.OperatingSystemVersion;
            deviceContext.Model = _environmentInformation.Model;
            await _registrationContext.SetDevice(deviceContext);
        }

        private async Task UpdateUserInContext(UserDetails user, string userId, bool isAnonymous)
        {
            var userContext = user ?? new UserDetails();
            userContext.UserId = userId;
            userContext.DisplayName = userContext.DisplayName ?? userId;
            userContext.IsAnonymous = isAnonymous;
            await _registrationContext.SetUser(userContext);
        }

        private static User CreateServiceUser(UserDetails user)
        {
            return new User
            {
                AdditionalProperties = user.AdditionalProperties,
                AvatarAssetId = user.AvatarAssetId,
                Id = user.UserId,
                CountryCode = user.CountryCode,
                DisplayName = user.DisplayName,
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MobileNumber = user.MobileNumber
            };
        }

        private string GetOrGenerateDeviceSecret()
        {
            if (string.IsNullOrEmpty(_registrationContext.DeviceSecret))
            {
                _registrationContext.DeviceSecret = Guid.NewGuid().ToString();
            }

            return _registrationContext.DeviceSecret;
        }

        private Dictionary<string, string> GetModuleVersions()
        {
            return _moduleManager.RegisteredModules
                .ToDictionary(m => m.Name, m => m.Version);
        }

        private void PublishRegistrationChanged()
        {
            PublishRegistrationChangedAsync().ExecuteInBackground();
        }

        private async Task PublishRegistrationChangedAsync()
        {
            var registrationDetails = await GetRegistrationDetailsAsync();
            if (registrationDetails != null)
            {
                await _eventBus.PublishAsync(new RegistrationChangedEvent(registrationDetails));
            }
        }


        public string NotificationSoundFilename { get; set; }
       
    }
}