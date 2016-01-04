// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DonkyCoreImplementation class.
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
using System.Threading;
using System.Threading.Tasks;
using Donky.Core.Configuration;
using Donky.Core.Diagnostics;
using Donky.Core.Events;
using Donky.Core.Exceptions;
using Donky.Core.Framework;
using Donky.Core.Framework.DependencyInjection;
using Donky.Core.Framework.Events;
using Donky.Core.Framework.Extensions;
using Donky.Core.Framework.Logging;
using Donky.Core.Notifications;
using Donky.Core.Registration;
using Donky.Core.Services;
using Donky.Core.Startup;

namespace Donky.Core
{
	internal class DonkyCoreImplementation : IDonkyCore
	{
		private readonly IRegistrationContext _registrationContext;
		private readonly IRegistrationManager _registrationManager;
		private readonly IModuleManager _moduleManager;
		private readonly IObjectBuilder _builder;
		private readonly IServiceContext _serviceContext;
		private readonly INotificationManager _notificationManager;
		private readonly IStartupManager _startupManager;
		private readonly IEventBus _eventBus;
		private readonly IDiagnosticsManager _diagnosticsManager;
		private readonly IConfigurationManager _configurationManager;
		private bool _isInitialised;
		private readonly SemaphoreSlim _initLock = new SemaphoreSlim(1);
		
		internal static readonly ModuleDefinition CoreModuleDefinition = new ModuleDefinition("DonkyDotNetCore", 
			AssemblyHelper.GetAssemblyVersion(typeof(IDonkyCore)).ToString());

		internal DonkyCoreImplementation(IRegistrationContext registrationContext, IRegistrationManager registrationManager, IModuleManager moduleManager, IObjectBuilder builder, IServiceContext serviceContext, INotificationManager notificationManager, IStartupManager startupManager, IEventBus eventBus, IDiagnosticsManager diagnosticsManager, IConfigurationManager configurationManager)
		{
			_registrationContext = registrationContext;
			_registrationManager = registrationManager;
			_moduleManager = moduleManager;
			_builder = builder;
			_serviceContext = serviceContext;
			_notificationManager = notificationManager;
			_startupManager = startupManager;
			_eventBus = eventBus;
			_diagnosticsManager = diagnosticsManager;
			_configurationManager = configurationManager;
		}

        public async Task<ApiResult> InitialiseAsync(string apiKey, UserDetails user = null, DeviceDetails device = null, string appVersion = null, string publicServiceBaseUrl = null, string notificationSoundFilename = null)
		{
			try
			{
				await _initLock.WaitAsync();

				return await ApiResult.ForOperationAsync(async () =>
				{
					if (_isInitialised)
					{
						throw new InvalidOperationException("Initialise should not be called more than once");
					}

					await _configurationManager.InitialiseAsync();

					_registrationContext.ApiKey = apiKey;
					_registrationContext.AppVersion = appVersion;

					RegisterInternalSubscriptions();

					if (!String.IsNullOrEmpty(publicServiceBaseUrl))
					{
						_serviceContext.PublicServiceBaseUrl = publicServiceBaseUrl;
					}

				    _registrationManager.NotificationSoundFilename = notificationSoundFilename;

					await _registrationManager.EnsureRegisteredAsync(user, device, appVersion, false);
					_isInitialised = true;

					var registration = await _registrationManager.GetRegistrationDetailsAsync();

					Logger.Instance.LogInformation("Initialised with user id {0}", 
						registration.UserDetails.UserId);

					_startupManager.PerformStartupTasksAsync().ExecuteInBackground();

				}, "InitialiseAsync");
			}
			finally
			{
				_initLock.Release();
			}
		}

		public IRegistrationController RegistrationController
		{
			get
			{
				ThrowIfNotInitialised();

				return _registrationManager;
			} 
		}

		public INotificationController NotificationController
		{
			get
			{
				ThrowIfNotInitialised();

				return _notificationManager;
			}
		}

		public ILoggingController LoggingController
		{
			get
			{
				ThrowIfNotInitialised();

				return _diagnosticsManager;				
			}
		}

		public void RegisterModule(ModuleDefinition definition)
		{
			if (_isInitialised)
			{
				throw new InvalidOperationException("RegisterModule should be called prior to initialisation.");
			}

		    if (definition != null)
		    {
		        definition.Validate();
		    }
		    else
		    {
                throw new ArgumentException("ArgumentException: Parameter 'definition' is null in method RegisterModule(ModuleDefinition definition) of DonkyCoreImplementation. [Donky.Core]");

		    }

		    _moduleManager.EnsureRegistered(definition);
		}

		public void SubscribeToNotifications(ModuleDefinition module, params CustomNotificationSubscription[] subscriptions)
		{
			if (_isInitialised)
			{
				throw new InvalidOperationException("SubscribeToNotifications should be called prior to initialisation.");
			}

			_notificationManager.SubscribeToNotifications(module, subscriptions);
		}

		public void SubscribeToOutboundNotifications(ModuleDefinition module, params OutboundNotificationSubscription[] subscriptions)
		{
			if (_isInitialised)
			{
				throw new InvalidOperationException("SubscribeToOutboundNotifications should be called prior to initialisation.");
			}

			_notificationManager.SubscribeToOutboundNotifications(module, subscriptions);
		}

		public void RegisterService<T>(T instance) where T : class
		{
			_builder.AddRegistration(instance);
		}

		public void RegisterServiceType<TService, TInstance>() where TService : class where TInstance : TService
		{
			_builder.AddRegistration<TService, TInstance>();
		}

		public T GetService<T>() where T : class
		{
			return _builder.BuildObject<T>();
		}

		public void SubscribeToLocalEvent<T>(Action<T> handler) where T : LocalEvent
		{
			_eventBus.Subscribe(handler);
		}

		public void UnsubscribeFromLocalEvent<T>(Action<T> handler) where T : LocalEvent
		{
			_eventBus.Unsubscribe(handler);
		}

		public void PublishLocalEvent<T>(T theEvent, ModuleDefinition publisher) where T : LocalEvent
		{
			theEvent.Publisher = publisher;
			_eventBus.PublishAsync(theEvent).ExecuteInBackground();
		}

		public bool IsInitialised
		{
			get { return _isInitialised; }
		}

		private void RegisterInternalSubscriptions()
		{
			_notificationManager.SubscribeToDonkyNotifications(
				CoreModuleDefinition,
				new DonkyNotificationSubscription
				{
					Type = "TransmitDebugLog",
					AutoAcknowledge = true,
					Handler = _diagnosticsManager.HandleTransmitDebugLogAsync
				},
				new DonkyNotificationSubscription
				{
					Type = "NewDeviceAddedToUser",
					AutoAcknowledge = true,
					Handler = not =>
					{
						PublishLocalEvent(new NewDeviceAddedEvent
						{
							Model = not.Data.Value<string>("model"),
							OperatingSystem = not.Data.Value<string>("operatingSystem")
						}, CoreModuleDefinition);

						return Task.FromResult(0);
					}
				}
				);
		}

		private void ThrowIfNotInitialised()
		{
			if (!_isInitialised)
			{
				throw new InvalidOperationException("Donky has not been initialised.");
			}
		}
	}
}