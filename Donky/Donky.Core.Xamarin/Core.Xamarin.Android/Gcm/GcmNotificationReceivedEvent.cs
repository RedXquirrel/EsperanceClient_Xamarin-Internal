// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     GcmNotificationReceivedEvent class.
//  Author:          Ben Moore
//  Created date:    13/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using Android.Content;
using Donky.Core.Events;

namespace Donky.Core.Xamarin.Android.Gcm
{
	/// <summary>
	/// LocalEvent that is raised when a GCM notification is received.
	/// </summary>
	public class GcmNotificationReceivedEvent : LocalEvent
	{
		public Intent Intent { get; set; }
	}
}