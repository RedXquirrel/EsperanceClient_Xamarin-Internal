// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     AppSession client notification
//  Author:          Ben Moore
//  Created date:    21/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using Donky.Core.Notifications;

namespace Donky.Core.Analytics.Notifications
{
	public class AppSessionClientNotification : ClientNotification
	{
		public AppSessionClientNotification()
		{
			Type = "AppSession";
		}

		public DateTime StartTimeUtc
		{
			get { return (DateTime)this["startTimeUtc"]; }
			set { this["startTimeUtc"] = value; }
		}

		public DateTime EndTimeUtc
		{
			get { return (DateTime)this["endTimeUtc"]; }
			set { this["endTimeUtc"] = value; }
		}

		public LaunchReason SessionTrigger
		{
			get { return (LaunchReason)this["sessionTrigger"]; }
			set { this["sessionTrigger"] = value; }
		}

		public string OperatingSystem
		{
			get { return (string)this["operatingSystem"]; }
			set { this["operatingSystem"] = value; }
		}
	}
}