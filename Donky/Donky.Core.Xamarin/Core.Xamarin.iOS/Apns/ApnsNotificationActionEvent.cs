// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ApnsNotificationActionEvent.
//  Author:          Ben Moore
//  Created date:    27/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using Donky.Core.Events;
using Foundation;

namespace Donky.Core.Xamarin.iOS.Apns
{
	/// <summary>
	/// Local Event raised when an APNS action is detected.
	/// </summary>
	public class ApnsNotificationActionEvent : LocalEvent
	{
		public ApnsNotificationActionEvent(string action, NSDictionary notificationInfo, Action completionHandler)
		{
			Action = action;
			NotificationInfo = notificationInfo;
			CompletionHandler = completionHandler;
		}

		public string Action { get; set; }

		public NSDictionary NotificationInfo { get; set; }

		public Action CompletionHandler { get; set; }
	}
}