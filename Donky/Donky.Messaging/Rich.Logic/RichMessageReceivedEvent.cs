// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     RichMessageReceivedEvent class.
//  Author:          Ben Moore
//  Created date:    07/07/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using Donky.Core.Events;

namespace Donky.Messaging.Rich.Logic
{
	/// <summary>
	/// Local Event indicating that a Rich Message has been received.
	/// </summary>
	public class RichMessageReceivedEvent : LocalEvent
	{
		/// <summary>
		/// The received message.
		/// </summary>
		public RichMessage Message { get; set; }

		public string NotificationId { get; set; }
	}
}