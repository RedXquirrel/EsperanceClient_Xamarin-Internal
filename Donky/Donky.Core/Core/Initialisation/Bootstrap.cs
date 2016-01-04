// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     Bootstrap class
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
using Autofac;
using Donky.Core.Assets;
using Donky.Core.Configuration;
using Donky.Core.Data;
using Donky.Core.Diagnostics;
using Donky.Core.Framework;
using Donky.Core.Framework.DependencyInjection;
using Donky.Core.Framework.Events;
using Donky.Core.Framework.Logging;
using Donky.Core.Framework.Network;
using Donky.Core.Framework.Storage;
using Donky.Core.Notifications;
using Donky.Core.Notifications.Remote;
using Donky.Core.Registration;
using Donky.Core.Services;
using Donky.Core.Services.Authentication;
using Donky.Core.Services.Diagnostics;
using Donky.Core.Services.Notification;
using Donky.Core.Services.Registration;
using Donky.Core.Startup;

namespace Donky.Core.Initialisation
{
	internal static class Bootstrap
	{
		internal static IObjectBuilder Builder;

		internal static void RegisterDependencies()
		{
			// Need to explicitly hook up internal constructors otherwise DI won't work
			RegisterDonkyApi();
			RegisterFramework();
			RegisterDataLayer();
			RegisterManagers();
			RegisterServices();
		}

		internal static void Initialise()
		{
			var builder = new AutofacObjectBuilder();
			RegisterDependencies();

			Builder = builder;
		}

		private static void RegisterServices()
		{
			AutofacObjectBuilder.ContainerBuilder.Register(
				c => new DonkyHttpClient())
				.As<IHttpClient>()
				.SingleInstance();

		    AutofacObjectBuilder.ContainerBuilder.Register(
		        c => new Logger(true))
		        .As<ILogger>()
		        .SingleInstance();

            AutofacObjectBuilder.ContainerBuilder.Register(
				c => new PublicRegistrationService(
					c.Resolve<IRegistrationContext>(),
					c.Resolve<IServiceContext>(),
					c.Resolve<IEnvironmentInformation>(),
					c.Resolve<ISdkInformation>(),
					c.Resolve<IHttpClient>(),
					c.Resolve<IConfigurationManager>()))
				.As<IPublicRegistrationService>()
				.SingleInstance();

			AutofacObjectBuilder.ContainerBuilder.Register(
				c => new SecureRegistrationService(
					c.Resolve<IRegistrationContext>(),
					c.Resolve<IServiceContext>(),
					c.Resolve<IEnvironmentInformation>(),
					c.Resolve<ISdkInformation>(),
					c.Resolve<IHttpClient>(), 
					c.Resolve<IConfigurationManager>()))
				.As<ISecureRegistrationService>()
				.SingleInstance();

			AutofacObjectBuilder.ContainerBuilder.Register(
				c => new NotificationService(
					c.Resolve<IRegistrationContext>(),
					c.Resolve<IServiceContext>(),
					c.Resolve<IEnvironmentInformation>(),
					c.Resolve<ISdkInformation>(),
					c.Resolve<IHttpClient>(),
					c.Resolve<IConfigurationManager>()))
				.As<INotificationService>()
				.SingleInstance();

			AutofacObjectBuilder.ContainerBuilder.Register(
				c => new DebugLogService(
					c.Resolve<IRegistrationContext>(),
					c.Resolve<IServiceContext>(),
					c.Resolve<IEnvironmentInformation>(),
					c.Resolve<ISdkInformation>(),
					c.Resolve<IHttpClient>(),
					c.Resolve<IConfigurationManager>()))
				.As<IDebugLogService>()
				.SingleInstance();

			AutofacObjectBuilder.ContainerBuilder.Register(
				c => c.Resolve<IPublicRegistrationService>())
				.As<IRefreshToken>()
				.SingleInstance();
		}

		private static void RegisterManagers()
		{
			AutofacObjectBuilder.ContainerBuilder.Register(
				c => new RegistrationManager(
					c.Resolve<IRegistrationContext>(),
					c.Resolve<IModuleManager>(),
					c.Resolve<IPublicRegistrationService>(),
					c.Resolve<ISdkInformation>(),
					c.Resolve<IEnvironmentInformation>(),
					c.Resolve<IServiceContext>(),
					c.Resolve<ISecureRegistrationService>(),
					c.Resolve<IConfigurationManager>(),
					c.Resolve<IEventBus>(),
                    c.Resolve<IRefreshToken>(),
                    c.Resolve<ILogger>()
                    ))
				.As<IRegistrationManager>()
				.SingleInstance();
	
			AutofacObjectBuilder.ContainerBuilder.Register(
				c => new ModuleManager())
				.As<IModuleManager>()
				.SingleInstance();	

			AutofacObjectBuilder.ContainerBuilder.Register(
				c => new NotificationManager(
					c.Resolve<IDonkyClientDataContext>(),
					c.Resolve<INotificationService>(),
					c.Resolve<IModuleManager>(),
					c.Resolve<IAppState>(),
					c.Resolve<IJsonSerialiser>(),
                    c.Resolve<ILogger>()
                    ))
				.As<INotificationManager>()
				.SingleInstance();

			AutofacObjectBuilder.ContainerBuilder.Register(c =>
				new RemoteNotificationManager(
					c.Resolve<IRemoteNotificationChannel>(),
					c.Resolve<IRegistrationManager>(),
					c.Resolve<IEventBus>(),
					c.Resolve<INotificationManager>()))
				.As<IRemoteNotificationManager>()
				.SingleInstance();

			AutofacObjectBuilder.ContainerBuilder.Register(c =>
				new StartupManager(
					c.Resolve<IRemoteNotificationManager>(),
					c.Resolve<INotificationManager>()))
				.As<IStartupManager>()
				.SingleInstance();

			AutofacObjectBuilder.ContainerBuilder.Register(c =>
				new DiagnosticsManager(
					c.Resolve<IDebugLogService>(),
					c.Resolve<IEventBus>(),
					c.Resolve<IConfigurationManager>(),
                    c.Resolve<ILogger>()))
				.As<IDiagnosticsManager>()
				.SingleInstance();

			AutofacObjectBuilder.ContainerBuilder.Register(c =>
				new ConfigurationManager(
					c.Resolve<IPersistentStorage>(),
					c.Resolve<IEventBus>(),
                    c.Resolve<ILogger>()))
				.As<IConfigurationManager>()
				.SingleInstance();

		}

		private static void RegisterDataLayer()
		{
			AutofacObjectBuilder.ContainerBuilder.Register(c =>
				new DonkyClientDataContext(
					c.Resolve<IPersistentStorage>(),
                    c.Resolve<ILogger>()
                    ))
				.As<IDonkyClientDataContext>()
				.SingleInstance();

			AutofacObjectBuilder.ContainerBuilder.Register(c =>
				new RegistrationContext(c.Resolve<IPersistentStorage>()))
				.As<IRegistrationContext>()
				.SingleInstance();

			AutofacObjectBuilder.ContainerBuilder.Register(c =>
				new ServiceContext(c.Resolve<IPersistentStorage>()))
				.As<IServiceContext>()
				.SingleInstance();


		}

		private static void RegisterFramework()
		{
			// Framework
			AutofacObjectBuilder.ContainerBuilder.Register(c => new JsonNetSerialiser()).As<IJsonSerialiser>()
				.SingleInstance();
			AutofacObjectBuilder.ContainerBuilder.Register(c => new EventBus()).As<IEventBus>()
				.SingleInstance();
			AutofacObjectBuilder.ContainerBuilder.Register(c => new SdkInformation()).As<ISdkInformation>()
				.SingleInstance();
			AutofacObjectBuilder.ContainerBuilder.Register(c => 
				new AppState(
					c.Resolve<IEventBus>()))
				.As<IAppState>()
				.SingleInstance();
			AutofacObjectBuilder.ContainerBuilder.Register(c => 
				new AssetHelper(
					c.Resolve<IConfigurationManager>()))
				.As<IAssetHelper>()
				.SingleInstance();
		}

		private static void RegisterDonkyApi()
		{
			AutofacObjectBuilder.ContainerBuilder.Register(c =>
				new DonkyCoreImplementation(
					c.Resolve<IRegistrationContext>(),
					c.Resolve<IRegistrationManager>(),
					c.Resolve<IModuleManager>(),
					c.Resolve<IObjectBuilder>(),
					c.Resolve<IServiceContext>(),
					c.Resolve<INotificationManager>(),
					c.Resolve<IStartupManager>(),
					c.Resolve<IEventBus>(),
					c.Resolve<IDiagnosticsManager>(),
					c.Resolve<IConfigurationManager>()))
				.As<IDonkyCore>()
				.SingleInstance();

			// Register the public controller interfaces as mappings
			AutofacObjectBuilder.ContainerBuilder.Register(c =>
				c.Resolve<IRegistrationManager>())
				.As<IRegistrationController>()
				.SingleInstance();

			AutofacObjectBuilder.ContainerBuilder.Register(c =>
				c.Resolve<INotificationManager>())
				.As<INotificationController>()
				.SingleInstance();

			AutofacObjectBuilder.ContainerBuilder.Register(c =>
				c.Resolve<IDiagnosticsManager>())
				.As<ILoggingController>()
				.SingleInstance();
		}
	}
}