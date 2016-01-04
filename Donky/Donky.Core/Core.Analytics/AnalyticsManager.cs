// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     AnalyticsManager class.
//  Author:          Ben Moore
//  Created date:    21/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Reflection;
using System.Threading.Tasks;
using Donky.Core.Analytics.Notifications;
using Donky.Core.Events;
using Donky.Core.Framework;
using Donky.Core.Framework.Extensions;
using Donky.Core.Framework.Storage;
using Donky.Core.Notifications;
using Donky.Core.Registration;

namespace Donky.Core.Analytics
{
    /// <summary>
    /// 
    /// </summary>
	internal class AnalyticsManager : IAnalyticsManager
	{
		private const string START_TIME_KEY = "DonkyCoreAnalytics_SessionStartTime";
		private const string SESSION_TRIGGER = "DonkyCoreAnalyics_SessionTrigger";

		private readonly INotificationManager _notificationManager;
		private readonly IEnvironmentInformation _environmentInformation;
		private readonly IPersistentStorage _persistentStorage;
		private readonly IRegistrationController _registrationController;

		public AnalyticsManager(INotificationManager notificationManager, IEnvironmentInformation environmentInformation, IPersistentStorage persistentStorage, IRegistrationController registrationController)
		{
			_notificationManager = notificationManager;
			_environmentInformation = environmentInformation;
			_persistentStorage = persistentStorage;
			_registrationController = registrationController;
		}

		public void HandleAppOpen(AppOpenEvent openEvent)
		{
			HandleAppOpenAsync(openEvent).ExecuteInBackground();
		}

		public void HandleAppClose(AppCloseEvent closeEvent)
		{
			HandleAppCloseAsync(closeEvent).ExecuteInBackground();
		}

		private async Task HandleAppOpenAsync(AppOpenEvent openEvent)
		{
			if (false == await _registrationController.GetIsRegisteredAsync())
			{
				return;
			}

			var notification = new AppLaunchClientNotification
			{
				LaunchTimeUtc = openEvent.Timestamp,
				OperatingSystem = _environmentInformation.OperatingSystem,
				SessionTrigger = openEvent.WasLaunchedFromNotification
					? LaunchReason.Notification 
					: LaunchReason.None
			};
			
			_persistentStorage.Set(START_TIME_KEY, notification.LaunchTimeUtc.ToString("O"));
			_persistentStorage.Set(SESSION_TRIGGER, notification.SessionTrigger.ToString());

			await _notificationManager.SendClientNotificationsAsync(notification);
		}

		private async Task HandleAppCloseAsync(AppCloseEvent closeEvent)
		{
			if (false == await _registrationController.GetIsRegisteredAsync())
			{
				return;
			}

			var rawStartTime = _persistentStorage.Get(START_TIME_KEY);
			DateTime? startTime = null;
			var trigger = LaunchReason.NotDefined;
			if (!String.IsNullOrEmpty(rawStartTime))
			{
				startTime = DateTime.Parse(rawStartTime);
				trigger = (LaunchReason) Enum.Parse(typeof (LaunchReason), _persistentStorage.Get(SESSION_TRIGGER));
			}

			if (startTime.HasValue)
			{
				var notification = new AppSessionClientNotification
				{
					StartTimeUtc = startTime.Value,
					EndTimeUtc = closeEvent.Timestamp,
					OperatingSystem = _environmentInformation.OperatingSystem,
					SessionTrigger = trigger
				};

				_persistentStorage.Set(START_TIME_KEY, String.Empty);
				_persistentStorage.Set(SESSION_TRIGGER, String.Empty);

				await _notificationManager.QueueClientNotificationAsync(notification);
			}
		}

	}
}