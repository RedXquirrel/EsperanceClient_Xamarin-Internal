// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ApnsRemoteNotificationChannel class.
//  Author:          Ben Moore
//  Created date:    18/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Threading.Tasks;
using Donky.Core.Framework.Events;
using Donky.Core.Framework.Extensions;
using Donky.Core.Framework.Logging;
using Donky.Core.Notifications.Remote;
using UIKit;

namespace Donky.Core.Xamarin.iOS.Apns
{
	/// <summary>
	/// APNS implementation of a remote notification channel
	/// </summary>
	public class ApnsRemoteNotificationChannel : IRemoteNotificationChannel
	{
		private readonly IEventBus _eventBus;

		public ApnsRemoteNotificationChannel(IEventBus eventBus)
		{
			_eventBus = eventBus;

			eventBus.Subscribe<ApnsRegistrationChangedEvent>(HandleApnsRegistrationChanged);
			eventBus.Subscribe<ApnsNotificationReceivedEvent>(HandleApnsNotificationReceived);
			eventBus.Subscribe<RefreshApnsConfigurationEvent>(HandleRefreshApnsConfiguration);
		}

		private void HandleRefreshApnsConfiguration(RefreshApnsConfigurationEvent theEvent)
		{
			RegisterForRemoteNotificationsAsync().ExecuteInBackground();
		}

		private void HandleApnsNotificationReceived(ApnsNotificationReceivedEvent apnsEvent)
		{
			Logger.Instance.LogDebug("Received APNS Push: {0}", apnsEvent.UserInfo.ToString());

			var notificationId = apnsEvent.UserInfo.GetValueOrDefault<string>("notificationId", null);
			
			if (notificationId != null)
			{
				var notificationEvent = new RemoteNotificationReceivedEvent
				{
					NotificationId = notificationId,
					NotificationType = "NOTIFICATIONPENDING",
					Publisher = DonkyiOS.Module
				};

				_eventBus.PublishAsync(notificationEvent).ExecuteInBackground();
			}

			if (apnsEvent.CompletionHandler != null)
			{
				apnsEvent.CompletionHandler(UIBackgroundFetchResult.NoData);
			}
		}

		private void HandleApnsRegistrationChanged(ApnsRegistrationChangedEvent apnsChangedEvent)
		{
			var token = apnsChangedEvent.Token
				.ToString()
				.Replace("<", String.Empty)
				.Replace(">", String.Empty)
				.Replace(" ", String.Empty);
			
			Logger.Instance.LogInformation("Received APNS token: {0}", token);

			var internalEvent = new RemoteChannelDetailsChanged
			{
				Details = new RemoteChannelDetails
				{
					Token = token
				},
				Publisher = DonkyiOS.Module
			};

			_eventBus.PublishAsync(internalEvent).ExecuteInBackground();
		}

		public async Task RegisterForRemoteNotificationsAsync()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				// Have to late bind to the configuration provider as it's configured after the main dependency build
				var configurationProvider = DonkyCore.Instance.GetService<IApnsConfigurationProvider>();
				var categories = await configurationProvider.GetRegisteredCategoriesAsync();

				UIApplication.SharedApplication.InvokeOnMainThread(() =>
				{
					var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
									   UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
									   categories);

					Logger.Instance.LogDebug("Calling APNS RegisterForRemoteNotifications with {0} categories", categories.Count);

					UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
					UIApplication.SharedApplication.RegisterForRemoteNotifications();
				});
			}
			else
			{
				Logger.Instance.LogDebug("Calling APNS RegisterForRemoteNotificationTypes");

				UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
				
				UIApplication.SharedApplication.InvokeOnMainThread(() => 
					UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes));
			}
		}
	}
}