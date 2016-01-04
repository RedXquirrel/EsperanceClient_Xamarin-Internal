// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DisplayModalPopupEvent class
//  Author:          Ben Moore
//  Created date:    07/07/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using Donky.Core.Events;
using Xamarin.Forms;

namespace Donky.Core.Xamarin.Forms.Alerts
{
	/// <summary>
	/// Local Event for displaying a modal popup
	/// </summary>
	public class DisplayModalPopupEvent : LocalEvent
	{
		public Page Page { get; set; }
	}
}