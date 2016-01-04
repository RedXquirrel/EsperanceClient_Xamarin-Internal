// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     GcmRemoteNotificationChannel class.
//  Author:          Ben Moore
//  Created date:    11/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Donky.Core.Configuration;
using Donky.Core.Framework;
using Donky.Core.Framework.Events;
using Donky.Core.Framework.Extensions;
using Donky.Core.Framework.Logging;
using Donky.Core.Notifications.Remote;

namespace Donky.Core.Xamarin.Android.Gcm
{
	/// <summary>
	/// GCM implementation of a RemoteNotificationChannel
	/// </summary>
	public class GcmRemoteNotificationChannel : IRemoteNotificationChannel
	{
		private readonly IEventBus _eventBus;
		private readonly IJsonSerialiser _serialiser;
		private readonly IConfigurationManager _configurationManager;

		public GcmRemoteNotificationChannel(IEventBus eventBus, IJsonSerialiser serialiser, IConfigurationManager configurationManager)
		{
			_eventBus = eventBus;
			_serialiser = serialiser;
			_configurationManager = configurationManager;

			eventBus.Subscribe<GcmNotificationReceivedEvent>(HandleGcmNotification);
		}

		private void HandleGcmNotification(GcmNotificationReceivedEvent theEvent)
		{
			RemoteNotificationReceivedEvent internalEvent = null;
			try
			{
				var notificationId = theEvent.Intent.GetStringExtra("notificationId");
				var payload = theEvent.Intent.GetStringExtra("payload");
				var notificationType = theEvent.Intent.GetStringExtra("type");
				var deserialisedPayload = _serialiser.Deserialise<Dictionary<string, object>>(payload);

				internalEvent = new RemoteNotificationReceivedEvent
				{
					NotificationId = notificationId,
					NotificationType = notificationType,
					Publisher = DonkyAndroid.Module,
					Payload = deserialisedPayload
				};
			}
			catch (Exception exception)
			{
				Logger.Instance.LogWarning("Could not parse GCM notification - may not be a Donky notification.  Details: {0}",
					exception.Message);				
			}

			if (internalEvent != null)

			{
				_eventBus.PublishAsync(internalEvent).ExecuteInBackground();
			}
		}

		public async Task RegisterForRemoteNotificationsAsync()
		{
			var context = Application.Context;

			var senderId = DonkyAndroid.Settings.GcmSenderId 
				?? _configurationManager.GetValue<string>("DefaultGCMSenderId");
			Logger.Instance.LogInformation("Registering for GCM with SenderId {0}", senderId);
			var senders = senderId;
			var intent = new Intent("com.google.android.c2dm.intent.REGISTER");
			intent.SetPackage("com.google.android.gsf");
			intent.PutExtra("app", PendingIntent.GetBroadcast(context, 0, new Intent(), 0));
			intent.PutExtra("sender", senders);
			context.StartService(intent);
		}
	}
}