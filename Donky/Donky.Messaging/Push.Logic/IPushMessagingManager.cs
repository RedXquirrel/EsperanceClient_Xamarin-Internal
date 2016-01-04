// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IPushMessagingManager interface.
//  Author:          Ben Moore
//  Created date:    14/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Threading.Tasks;
using Donky.Core.Notifications;

namespace Donky.Messaging.Push.Logic
{
	/// <summary>
	/// Push messaging operations 
	/// </summary>
	public interface IPushMessagingManager
	{
		/// <summary>
		/// Handles a simple push server notification.
		/// </summary>
		/// <param name="notification">The notification.</param>
		/// <returns></returns>
		Task HandleSimplePushAsync(ServerNotification notification);

		/// <summary>
		/// Handles the result of interaction / button press.
		/// </summary>
		/// <param name="messageId">The message identifier.</param>
		/// <param name="interactionType">Type of the interaction.</param>
		/// <param name="buttonDescription">The button description.</param>
		/// <param name="userAction">The user action.</param>
		/// <returns></returns>
		Task HandleInteractionResultAsync(Guid messageId, string interactionType, string buttonDescription, string userAction);
	}
}