// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     SimplePushMessage class.
//  Author:          Ben Moore
//  Created date:    15/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using Donky.Messaging.Common;

namespace Donky.Messaging.Push.Logic
{
	/// <summary>
	/// Definition of a Simple Push message from the Donky Network
	/// </summary>
	public class SimplePushMessage : Message
	{
		/// <summary>
		/// Details of any interactive buttons attached to the push message.
		/// </summary>
		public List<ButtonSet> ButtonSets { get; set; }		 
	}
}