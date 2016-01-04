// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DisplayAlertEvent class
//  Author:          Ben Moore
//  Created date:    28/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using Donky.Core.Events;
using Xamarin.Forms;

namespace Donky.Core.Xamarin.Forms.Alerts
{
	/// <summary>
	/// LocalEvent for displaying an alert.
	/// </summary>
	public class DisplayAlertEvent : LocalEvent
	{
		/// <summary>
		/// The content to be display as an alert.
		/// </summary>
		public View Content { get; set; }

		/// <summary>
		/// If true, the alert will be auto dismissed after an interval.
		/// </summary>
		public bool AutoDismiss { get; set; }

		/// <summary>
		/// The time the alert will be displayed
		/// </summary>
		public TimeSpan? DisplayTime { get; set; }

		/// <summary>
		/// Dismisses this alert.
		/// </summary>
		public void Dismiss()
		{
			var dismissed = Dismissed;
			if (dismissed != null)
			{
				dismissed(this, new EventArgs());
			}
		}

		/// <summary>
		/// Raised when the alert is dismissed.
		/// </summary>
		public event EventHandler Dismissed;
	}
}