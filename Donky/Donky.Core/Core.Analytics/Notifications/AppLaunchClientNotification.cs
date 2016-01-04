// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     AppLaunch Client Notification
//  Author:          Ben Moore
//  Created date:    21/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using Donky.Core.Notifications;

namespace Donky.Core.Analytics.Notifications
{
	public class AppLaunchClientNotification : ClientNotification
	{
		public AppLaunchClientNotification()
		{
			Type = "AppLaunch";
		}

		public DateTime LaunchTimeUtc
		{
			get { return (DateTime)this["launchTimeUtc"]; }
			set { this["launchTimeUtc"] = value; }
		}

		public LaunchReason SessionTrigger
		{
			get { return (LaunchReason) this["sessionTrigger"]; }
			set { this["sessionTrigger"] = value; }
		}

		public string OperatingSystem
		{
			get { return (string)this["operatingSystem"]; }
			set { this["operatingSystem"] = value; }
		}
	}
}