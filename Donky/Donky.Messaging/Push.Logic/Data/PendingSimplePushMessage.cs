// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     PendingSimplePushMessage class.
//  Author:          Ben Moore
//  Created date:    27/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using Donky.Core.Framework.Data;

namespace Donky.Messaging.Push.Logic.Data
{
	/// <summary>
	/// Details of a push message which is awaiting an interaction result.
	/// </summary>
	public class PendingSimplePushMessage : IEntity<Guid>
	{
		/// <summary>
		/// The unique id for this entity.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		public SimplePushMessage Message { get; set; }
	}
}