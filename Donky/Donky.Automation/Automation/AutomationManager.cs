// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     Automation Manager
//  Author:          Ben Moore
//  Created date:    22/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Donky.Automation.Notifications;
using Donky.Core.Framework.Extensions;
using Donky.Core.Notifications;

namespace Donky.Automation
{
	/// <summary>
	/// Logic for Automation
	/// </summary>
	/// <remarks>
	/// Handles execution of 3rd party triggers from the client.
	/// </remarks>
	internal class AutomationManager : IAutomationManager
	{
		private readonly INotificationManager _notificationManager;

		public AutomationManager(INotificationManager notificationManager)
		{
			_notificationManager = notificationManager;
		}

		/// <summary>
		/// Executes the third party trigger.
		/// </summary>
		/// <param name="triggerKey">The trigger key.</param>
		/// <param name="customData">The custom data.</param>
		/// <param name="sendImmediately">if set to <c>true</c> the generated client notification will be sent immediately.</param>
		public void ExecuteThirdPartyTrigger(string triggerKey, Dictionary<string, string> customData = null, bool sendImmediately = false)
		{
			ExecuteThirdPartyTriggerAsync(triggerKey, customData, sendImmediately).ExecuteInBackground();
		}

		private async Task ExecuteThirdPartyTriggerAsync(string triggerKey, Dictionary<string, string> customData, bool sendImmediately)
		{
			var notification = new ExecuteThirdPartyTriggersNotification
			{
				TriggerKey = triggerKey,
				Timestamp = DateTime.UtcNow,
				CustomData = customData
			};

			await _notificationManager.QueueClientNotificationAsync(notification);
			if (sendImmediately)
			{
				await _notificationManager.SynchroniseAsync();
			}
		}
	}
}