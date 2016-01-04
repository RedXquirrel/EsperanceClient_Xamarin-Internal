// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DonkyiOS class.
//  Author:          Ben Moore
//  Created date:    06/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using Donky.Core.Events;
using Donky.Core.Framework;
using Donky.Core.Framework.Logging;
using Donky.Core.Framework.Storage;
using Donky.Core.Notifications.Remote;
using Donky.Core.Registration;
using Donky.Core.Xamarin.iOS.Apns;
using Donky.Core.Xamarin.iOS.Storage;
using Foundation;
using UIKit;
using System.Diagnostics;

namespace Donky.Core.Xamarin.iOS
{
	/// <summary>
	/// Donky iOS Module
	/// </summary>
	public static class DonkyiOS
	{
		public static readonly ModuleDefinition Module = new ModuleDefinition(
			"DonkyXamariniOSCore", AssemblyHelper.GetAssemblyVersion(typeof(DonkyiOS)).ToString());

		/// <summary>
		/// Initialises the iOS Donky Module.
		/// </summary>
		public static void Initialise()
		{
			DonkyCore.Registry.AddRegistration<IEnvironmentInformation, iOSEnvironmentInformation>();
			DonkyCore.Registry.AddRegistration<IPersistentStorage, iOSPersistentStorage>();
			DonkyCore.Registry.AddRegistration<IFileStorageFactory, iOSFileStorageFactory>();
			DonkyCore.Registry.AddRegistration<IRemoteNotificationChannel, ApnsRemoteNotificationChannel>();

			DonkyCore.Instance.RegisterModule(Module);
			DonkyCore.Instance.RegisterServiceType<IAssetDataProvider, AssetDataProvider>();
			DonkyCore.Instance.RegisterServiceType<IApnsConfigurationProvider, ApnsConfigurationProvider>();

			DonkyCore.Instance.SubscribeToLocalEvent<DecrementBadgeCountEvent>(e =>
			{
				UIApplication.SharedApplication.InvokeOnMainThread(() =>
				{
					if (UIApplication.SharedApplication.ApplicationIconBadgeNumber > 0)
					{
						UIApplication.SharedApplication.ApplicationIconBadgeNumber--;
					}
				});
			});
		}

		public static void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			DonkyCore.Instance.PublishLocalEvent(new ApnsRegistrationChangedEvent(deviceToken), Module);
		}

		public static void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo,
			Action<UIBackgroundFetchResult> completionHandler)
		{
			var state = UIApplication.SharedApplication.ApplicationState;
			Logger.Instance.LogDebug("DidReceiveRemoteNotification, state: {0}", state);
			SetLaunchState(state, userInfo);
			DonkyCore.Instance.PublishLocalEvent(new ApnsNotificationReceivedEvent(userInfo, completionHandler), Module);			
		}

		public static void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			Logger.Instance.LogWarning("Failed to register for remote notifications: {0}", error.ToString());
		}

		public static void HandleAction(UIApplication application, string actionIdentifier, NSDictionary remoteNotificationInfo, Action completionHandler)
		{
			Logger.Instance.LogInformation("Handling notification action {0}.  State: {1}",
				actionIdentifier, application.ApplicationState);
			SetLaunchState(application.ApplicationState, remoteNotificationInfo);
			DonkyCore.Instance.PublishLocalEvent(new ApnsNotificationActionEvent(actionIdentifier, remoteNotificationInfo, completionHandler), Module);
		}

		private static void SetLaunchState(UIApplicationState state, NSDictionary userInfo)
		{
			if (state != UIApplicationState.Active)
			{
				var appState = DonkyCore.Instance.GetService<IAppState>();
				appState.WasOpenedFromNotification = true;
				appState.LaunchingNotificationId = userInfo.GetValueOrDefault<string>("notificationId", null);
			}
		}
	}
}