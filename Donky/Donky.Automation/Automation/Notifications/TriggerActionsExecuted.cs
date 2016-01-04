using System.Collections.Generic;

namespace Donky.Automation.Notifications
{
	public class TriggerActionsExecuted
	{
		public string TriggerId { get; set; }

		public List<TriggerActionExecutedData> ActionsExecuted { get; set; }
	}
}