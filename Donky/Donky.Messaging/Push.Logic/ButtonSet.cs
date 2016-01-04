// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ButtonSet class.
//  Author:          Ben Moore
//  Created date:    25/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;

namespace Donky.Messaging.Push.Logic
{
	/// <summary>
	/// Interactive buttonset info (from Donky Network)
	/// </summary>
	public class ButtonSet
	{
		/// <summary>
		/// Gets or sets the button set identifier.
		/// </summary>
		public string ButtonSetId { get; set; }

		/// <summary>
		/// Gets or sets the platform.
		/// </summary>
		public string Platform { get; set; }

		/// <summary>
		/// Gets or sets the type of the interaction.
		/// </summary>
		/// <value>
		/// The type of the interaction.
		/// </value>
		public string InteractionType { get; set; }

		/// <summary>
		/// Gets or sets the actions.
		/// </summary>
		public IList<ButtonSetAction> ButtonSetActions { get; set; }
	}
}