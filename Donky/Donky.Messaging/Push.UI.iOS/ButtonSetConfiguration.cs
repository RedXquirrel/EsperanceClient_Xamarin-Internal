// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ButtonSetConfiguration class.
//  Author:          Ben Moore
//  Created date:    27/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
namespace Donky.Messaging.Push.UI.iOS
{
	/// <summary>
	/// Configuration for a button set from the Donky Network.
	/// </summary>
	public class ButtonSetConfiguration
	{
		/// <summary>
		/// The button set id.
		/// </summary>
		public string ButtonSetId { get; set; }

		/// <summary>
		/// The button values for this set.
		/// </summary>
		public string[] ButtonValues { get; set; }
	}
}