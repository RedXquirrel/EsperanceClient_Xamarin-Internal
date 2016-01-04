// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ApnsNotificationReceivedEvent class
//  Author:          Ben Moore
//  Created date:    18/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using Donky.Core.Events;
using Foundation;
using UIKit;

namespace Donky.Core.Xamarin.iOS.Apns
{
	/// <summary>
	/// Local Event raised when an APNS notification is received.
	/// </summary>
	public class ApnsNotificationReceivedEvent : LocalEvent
	{
		public ApnsNotificationReceivedEvent(NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			UserInfo = userInfo;
			CompletionHandler = completionHandler;
		}

		public NSDictionary UserInfo { get; set; }

		public Action<UIBackgroundFetchResult> CompletionHandler { get; set; }
	}
}