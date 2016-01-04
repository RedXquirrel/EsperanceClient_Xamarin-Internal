// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DonkyAndroid class
//  Author:          Ben Moore
//  Created date:    06/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using System;
using Android.App;
using Android.Content;
using Donky.Core.Events;
using Donky.Core.Framework;
using Donky.Core.Framework.Logging;
using Donky.Core.Framework.Storage;
using Donky.Core.Notifications.Remote;
using Donky.Core.Registration;
using Donky.Core.Xamarin.Android.Gcm;
using Donky.Core.Xamarin.Android.Storage;

namespace Donky.Core.Xamarin.Android
{
	/// <summary>
	/// Core Donky Android module
	/// </summary>
	public static class DonkyAndroid
	{
		public static readonly ModuleDefinition Module = new ModuleDefinition(
			"DonkyXamarinAndroidCore", AssemblyHelper.GetAssemblyVersion(typeof(DonkyAndroid)).ToString());

		private static readonly DonkyAndroidSettings _settings = new DonkyAndroidSettings();

		public const string DonkyNotificationId = "DonkyNotificationId";

		/// <summary>
		/// Initialises the module.
		/// </summary>
		public static void Initialise()
		{
			DonkyCore.Registry.AddRegistration<IEnvironmentInformation, AndroidEnvironmentInformation>();
			DonkyCore.Registry.AddRegistration<IPersistentStorage, AndroidPersistentStorage>();
			DonkyCore.Registry.AddRegistration<IFileStorageFactory, AndroidFileStorageFactory>();
			DonkyCore.Registry.AddRegistration<IRemoteNotificationChannel, GcmRemoteNotificationChannel>();

			DonkyCore.Instance.RegisterModule(Module);
			DonkyCore.Instance.RegisterServiceType<IAssetProvider, AssetProvider>();
			DonkyCore.Instance.SubscribeToLocalEvent<NewDeviceAddedEvent>(HandleNewDeviceAdded);
		}

		private static void HandleNewDeviceAdded(NewDeviceAddedEvent newDeviceEvent)
		{
			if (Settings.NewDeviceNotificationEnabled)
			{
				// Setup intent to launch application
				var context = Application.Context;

				// Build the notification
				var builder = new Notification.Builder(context)
					.SetContentTitle(Settings.NewDeviceNotificateTitle)
					.SetSmallIcon(Resource.Drawable.ic_donky_new_device_default)
					.SetContentText(String.Format(Settings.NewDeviceNotificationMessageFormat,
						newDeviceEvent.OperatingSystem, newDeviceEvent.Model))
					.SetAutoCancel(true);

				var notification = builder.Build();

				var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
				notificationManager.Notify(new Random().Next(), notification);
			}
		}

		public static DonkyAndroidSettings Settings
		{
			get { return _settings; }
		}

		public static void ActivityLaunchedWithIntent(Activity activity)
		{
			if (activity.Intent != null && activity.Intent.Extras != null
			    && activity.Intent.Extras.ContainsKey(DonkyNotificationId))
			{
				var state = DonkyCore.Instance.GetService<IAppState>();
				state.WasOpenedFromNotification = true;
				state.LaunchingNotificationId = activity.Intent.Extras.GetString(DonkyNotificationId);
				Logger.Instance.LogDebug("Launched from notification {0}", state.LaunchingNotificationId);
			}
		}
	}
}