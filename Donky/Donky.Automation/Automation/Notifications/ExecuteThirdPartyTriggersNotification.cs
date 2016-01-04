using System;
using System.Collections.Generic;
using Donky.Core.Notifications;

namespace Donky.Automation.Notifications
{
	public class ExecuteThirdPartyTriggersNotification : ClientNotification
	{
        // ToDo: Later Release: "Add location support when CoreLocation module is implemented."

		public ExecuteThirdPartyTriggersNotification()
		{
			Type = "ExecuteThirdPartyTriggers";
		}

		public string TriggerKey
		{
			get { return (string) this["triggerKey"]; }
			set { this["triggerKey"] = value; }
		}

		public DateTime Timestamp
		{
			get { return (DateTime)this["timestamp"]; }
			set { this["timestamp"] = value; }
		}

		public List<TriggerActionsExecuted> TriggerActionsExecuted
		{
			get { return (List<TriggerActionsExecuted>)this["triggerActionsExecuted"]; }
			set { this["triggerActionsExecuted"] = value; }
		}

		public Dictionary<string, string> CustomData
		{
			get { return (Dictionary<string, string>)this["customData"]; }
			set { this["customData"] = value; }
		}
	}
}