// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     RemoteNotificationManager class.
//  Author:          Ben Moore
//  Created date:    11/05/2015
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
using System.Threading.Tasks;
using Donky.Core.Framework.Events;
using Donky.Core.Framework.Extensions;
using Donky.Core.Framework.Logging;
using Donky.Core.Registration;

namespace Donky.Core.Notifications.Remote
{
	/// <summary>
	/// Logic for handling remote notification channels.
	/// </summary>
	internal class RemoteNotificationManager : IRemoteNotificationManager
	{
		private readonly IRemoteNotificationChannel _channel;
		private readonly IRegistrationManager _registrationManager;
		private readonly INotificationManager _notificationManager;
		
		public RemoteNotificationManager(IRemoteNotificationChannel channel, IRegistrationManager registrationManager, IEventBus eventBus, INotificationManager notificationManager)
		{
			_channel = channel;
			_registrationManager = registrationManager;
			_notificationManager = notificationManager;

			eventBus.Subscribe<RemoteChannelDetailsChanged>(HandleChannelDetailsChanged);
			eventBus.Subscribe<RemoteNotificationReceivedEvent>(HandleNotificationReceived);
		}

		private void HandleNotificationReceived(RemoteNotificationReceivedEvent notificationEvent)
		{
			HandleNotificationReceivedInternal(notificationEvent).ExecuteInBackground();
		}

		private async Task HandleNotificationReceivedInternal(RemoteNotificationReceivedEvent notificationEvent)
		{
			Logger.Instance.LogInformation("Processing remote notification of type {0}",
				notificationEvent.NotificationType);

			if (notificationEvent.NotificationType == "NOTIFICATIONPENDING")
			{
				if (!String.IsNullOrEmpty(notificationEvent.NotificationId))
				{
					await _notificationManager.GetAndProcessNotificationAsync(notificationEvent.NotificationId);
				}
				else
				{
					Logger.Instance.LogDebug("No notification id found on remote notification - performing a full sync instead");
					await _notificationManager.SynchroniseAsync();
				}
			}
			else
			{
				Logger.Instance.LogError("Unexpected notification type {0} from remote notification",
					notificationEvent.NotificationType);
				throw new InvalidOperationException("Unexpected notification type");
			}
		}

		public async Task RegisterForRemoteNotificationsAsync()
		{
			Logger.Instance.LogDebug("Attempting to register for remote notifications");
			await _channel.RegisterForRemoteNotificationsAsync();
		}

		private void HandleChannelDetailsChanged(RemoteChannelDetailsChanged remoteChannelDetailsChanged)
		{
			_registrationManager.UpdatePushRegistrationAsync(remoteChannelDetailsChanged.Details)
				.ExecuteInBackground();
		}
	}
}