// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DonkyRichPopupUIAndroid class.
//  Author:          Ben Moore
//  Created date:    31/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Donky.Core;
using Donky.Core.Framework;
using Donky.Core.Framework.Logging;
using Donky.Core.Registration;
using Donky.Core.Xamarin.Android;
using Donky.Messaging.Rich.Logic;

namespace Donky.Messaging.Rich.PopupUI.Android
{
	/// <summary>
	/// Entry point for the Donky Push UI for Android
	/// </summary>
	public static class DonkyRichPopupUIAndroid
	{
		public static readonly ModuleDefinition Module = new ModuleDefinition(
			"DonkyAndroidRichPopupUI", AssemblyHelper.GetAssemblyVersion(typeof(DonkyRichPopupUIAndroid)).ToString());

		private static bool _isInitialised;
		private static readonly object Lock = new object();

		/// <summary>
		/// Initialises this module.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">DonkyAndroidRichPopupUI is already initialised</exception>
		public static void Initialise()
		{
			lock (Lock)
			{
				if (_isInitialised)
				{
					throw new InvalidOperationException("DonkyAndroidRichPopupUI is already initialised");
				}

				DonkyCore.Instance.RegisterModule(Module);
				DonkyCore.Instance.SubscribeToLocalEvent<RichMessageReceivedEvent>(HandleRichMessageReceived);

				_isInitialised = true;
			}
		}
	
		private static void HandleRichMessageReceived(RichMessageReceivedEvent messageEvent)
		{
			if (!DonkyCore.Instance.GetService<IAppState>().IsOpen)
			{
				ShowLocalNotificationForMessage(messageEvent);
			}
		}

		private static void ShowLocalNotificationForMessage(RichMessageReceivedEvent messageEvent)
		{
			var message = messageEvent.Message;
			var nativeNotificationId = new Random().Next();

			// Setup intent to launch application
			var context = Application.Context;

			// Build the notification
			var builder = new Notification.Builder(context)
				.SetContentTitle(message.SenderDisplayName)
				.SetSmallIcon(Resource.Drawable.donky_notification_small_icon_rich)
				.SetContentText(message.Description);
			var intent = CreateIntent(context, nativeNotificationId, messageEvent.NotificationId);
			builder.SetContentIntent(intent)
				.SetAutoCancel(true);

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				builder.SetCategory(Notification.CategoryMessage)
					.SetPriority((int) NotificationPriority.High)
					.SetVisibility(NotificationVisibility.Public);
			}

			var notification = builder.Build();

			var notificationManager = (NotificationManager) context.GetSystemService(Context.NotificationService);
			notificationManager.Notify(nativeNotificationId, notification);
		}

		private static PendingIntent CreateIntent(Context context, int nativeNotificationId, string donkyNotificationId)
		{
			var packageName = context.PackageName;
			var packageManager = context.PackageManager;

			var intent = packageManager.GetLaunchIntentForPackage(packageName);

			if (Build.VERSION.SdkInt >= BuildVersionCodes.HoneycombMr1)
			{
				intent.AddFlags(ActivityFlags.IncludeStoppedPackages);
			}

			intent.PutExtra(DonkyAndroid.DonkyNotificationId, donkyNotificationId);

			return PendingIntent.GetActivity(context, nativeNotificationId, intent, PendingIntentFlags.UpdateCurrent);
		}
	}
}