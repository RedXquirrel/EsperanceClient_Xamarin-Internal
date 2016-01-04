// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IPushDataContext interface.
//  Author:          Ben Moore
//  Created date:    27/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using Donky.Core.Framework.Data;

namespace Donky.Messaging.Push.Logic.Data
{
	/// <summary>
	/// Data context for temporary storage and coordination of interactive messages
	/// </summary>
	public interface IPushDataContext : IDataContext
	{
		/// <summary>
		/// Messages that are awaiting interaction results.
		/// </summary>
		IEntitySet<PendingSimplePushMessage, Guid> SimplePushMessages { get; }

		/// <summary>
		/// Interaction results that are awaiting message info.
		/// </summary>
		IEntitySet<PendingMessageInteraction, Guid> MessageInteractions { get; } 
	}
}