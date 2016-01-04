// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IRichMessagingManager interface.
//  Author:          Ben Moore
//  Created date:    03/07/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Donky.Core.Notifications;

namespace Donky.Messaging.Rich.Logic
{
	/// <summary>
	/// Logic operations for Rich Messages
	/// </summary>
	public interface IRichMessagingManager
	{
		Task<IEnumerable<RichMessage>> GetAllAsync();

		Task<IEnumerable<Logic.RichMessage>> GetAllUnreadAsync();

		Task<RichMessage> GetByIdAsync(Guid messageId);

		Task MarkMessageAsReadAsync(Guid messageId);

		Task DeleteMessagesAsync(params Guid[] messageIds);

		Task HandleRichMessageAsync(ServerNotification notification);
	}
}