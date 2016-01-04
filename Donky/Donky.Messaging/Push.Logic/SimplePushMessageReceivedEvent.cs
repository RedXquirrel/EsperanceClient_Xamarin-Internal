// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     SimplePushMessageReceivedEvent class.
//  Author:          Ben Moore
//  Created date:    21/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using Donky.Core.Events;

namespace Donky.Messaging.Push.Logic
{
	/// <summary>
	/// LocalEvent that is published when a Simple Push message is received
	/// </summary>
	public class SimplePushMessageReceivedEvent : LocalEvent
	{
		/// <summary>
		/// The id of the Donky Notification that delivered the message.
		/// </summary>
		public string NotificationId { get; set; }

		/// <summary>
		/// Details of the message
		/// </summary>
		public SimplePushMessage Message { get; set; }

		/// <summary>
		/// The button set from the message definition that is applicable for the current platform (if any)
		/// </summary>
		public ButtonSet PlatformButtonSet { get; set; }
	}
}