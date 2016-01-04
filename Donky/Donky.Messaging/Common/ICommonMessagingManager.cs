// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ICommonMessagingManager interface.
//  Author:          Ben Moore
//  Created date:    14/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System.Threading.Tasks;
using Donky.Core.Notifications;

namespace Donky.Messaging.Common
{
	/// <summary>
	/// Common messaging operations.
	/// </summary>
	public interface ICommonMessagingManager
	{
		/// <summary>
		/// Notifies the Donky Network that a message has been received.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="serverNotification">The server notification.</param>
		/// <returns></returns>
		Task NotifyMessageReceivedAsync(Message message, ServerNotification serverNotification);

		/// <summary>
		/// Notifies the Donky Network that a message has been read.
		/// </summary>
		/// <param name="message">The message.</param>
		Task NotifyMessageReadAsync(Message message);
	}
}