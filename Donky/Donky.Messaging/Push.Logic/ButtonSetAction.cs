// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ButtonSetAction class.
//  Author:          Ben Moore
//  Created date:    25/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
namespace Donky.Messaging.Push.Logic
{
	/// <summary>
	/// Defines an action included in a button set.
	/// </summary>
	public class ButtonSetAction
	{
		/// <summary>
		/// The type of action.
		/// </summary>
		public string ActionType { get; set; }

		/// <summary>
		/// Any data associated with the action (e.g. Deep Link values)
		/// </summary>
		public string Data { get; set; }

		/// <summary>
		/// The label for the action button
		/// </summary>
		public string Label { get; set; }
	}
}