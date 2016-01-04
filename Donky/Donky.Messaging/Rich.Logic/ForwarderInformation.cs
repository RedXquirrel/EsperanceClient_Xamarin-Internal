// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ForwarderInformation class.
//  Author:          Ben Moore
//  Created date:    06/07/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;

namespace Donky.Messaging.Rich.Logic
{
	/// <summary>
	/// Information about the forwarder of a message.
	/// </summary>
	public class ForwarderInformation
	{
		/// <summary>
		/// Display name of the user doing the forwarding
		/// </summary>
		public string DisplayName { get; set; }

		/// <summary>
		/// Internal user id of the user doing the forwarding
		/// </summary>
		public Guid InternalUserId { get; set; }

		/// <summary>
		/// External user id of the user doing the forwarding
		/// </summary>
		public string ExternalUserId { get; set; }
	}
}