// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ApnsRegistrationChangedEvent class.
//  Author:          Ben Moore
//  Created date:    18/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using Donky.Core.Events;
using Foundation;

namespace Donky.Core.Xamarin.iOS.Apns
{
	/// <summary>
	/// Local Event raised when APNS registration details change
	/// </summary>
	public class ApnsRegistrationChangedEvent : LocalEvent
	{
		public ApnsRegistrationChangedEvent(NSData token)
		{
			Token = token;
		}

		public NSData Token { get; set; }
	}
}