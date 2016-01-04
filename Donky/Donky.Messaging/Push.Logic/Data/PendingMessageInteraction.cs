// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     PendingMessageInteraction class.
//  Author:          Ben Moore
//  Created date:    27/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using Donky.Core.Framework.Data;

namespace Donky.Messaging.Push.Logic.Data
{
	/// <summary>
	/// Details of a message interaction
	/// </summary>
	public class PendingMessageInteraction : IEntity<Guid>
	{
		/// <summary>
		/// The unique id for this entity.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the type of the interaction.
		/// </summary>
		public string InteractionType { get; set; }

		/// <summary>
		/// Gets or sets the button description.
		/// </summary>
		public string ButtonDescription { get; set; }

		/// <summary>
		/// Gets or sets the user action.
		/// </summary>
		public string UserAction { get; set; }

		/// <summary>
		/// Gets or sets the interaction time.
		/// </summary>
		public DateTime InteractionTime { get; set; }
	}
}