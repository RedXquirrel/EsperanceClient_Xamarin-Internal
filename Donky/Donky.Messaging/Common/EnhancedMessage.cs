// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     EnhancedMessage class.
//  Author:          Ben Moore
//  Created date:    06/07/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace Donky.Messaging.Common
{
	/// <summary>
	/// Enhanced message definition which contains additional data
	/// </summary>
	public abstract class EnhancedMessage : Message
	{
		/// <summary>
		/// The Conversation Id, used to group messages into threads
		/// </summary>
		public string ConversationId { get; set; }

		/// <summary>
		/// Details of any related assets / attachments
		/// </summary>
		public IList<Asset> Assets { get; set; }

		/// <summary>
		/// The type of the account that sent the message.
		/// </summary>
		public SenderType SenderAccountType { get; set; }

		/// <summary>
		/// The user id of the sender.
		/// </summary>
		public string SenderExternalUserId { get; set; }

		/// <summary>
		/// Optional external reference for the message.
		/// </summary>
		public string ExternalRef { get; set; }

		/// <summary>
		/// Indicates that the message can be replied to.
		/// </summary>
		public bool CanReply { get; set; }

		/// <summary>
		/// Indicates that the message should be delivered without any visible or audible alert to the user.
		/// </summary>
		public bool SilentNotification { get; set; }

		/// <summary>
		/// The id of the app user account that replies should be directed to
		/// </summary>
		public string ReplyToUserId { get; set; }
	}
}