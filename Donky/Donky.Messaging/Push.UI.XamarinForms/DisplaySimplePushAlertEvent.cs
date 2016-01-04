// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DisplaySimplePushAlertEvent class
//  Author:          Ben Moore
//  Created date:    31/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using Donky.Core.Events;
using Donky.Messaging.Push.Logic;

namespace Donky.Messaging.Push.UI.XamarinForms
{
	/// <summary>
	/// LocalEvent that is used to request the display of an alert for a simple push message.
	/// </summary>
	public class DisplaySimplePushAlertEvent : LocalEvent
	{
		/// <summary>
		/// The event raised when the message was originally received
		/// </summary>
		public SimplePushMessageReceivedEvent MessageReceivedEvent { get; set; }
	}
}