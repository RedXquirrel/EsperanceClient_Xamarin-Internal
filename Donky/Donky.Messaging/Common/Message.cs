// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     Message class.
//  Author:          Ben Moore
//  Created date:    15/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;

namespace Donky.Messaging.Common
{
	/// <summary>
	/// Basic definition of a Donky Message
	/// </summary>
	public abstract class Message
	{
		/// <summary>
		/// The name to display for the sender.
		/// </summary>
		public string SenderDisplayName { get; set; }

		/// <summary>
		/// The message body
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		/// The message scope (A2P or P2P).
		/// </summary>
		public string MessageScope { get; set; }

		/// <summary>
		/// The type of the message (Rich, Chat, SimplePush)
		/// </summary>
		public string MessageType { get; set; }

		/// <summary>
		/// The internal Donky Id for the sender of the message
		/// </summary>
		public Guid SenderInternalUserId { get; set; }

		/// <summary>
		/// The correlation id used for sending receipts back to the originator
		/// </summary>
		public string SenderMessageId { get; set; }

		/// <summary>
		/// The unique id for this message.
		/// </summary>
		public Guid MessageId { get; set; }

		/// <summary>
		/// Any additional context that is carried with the message.
		/// </summary>
		public Dictionary<string, string> ContextItems { get; set; }

		/// <summary>
		/// The Donky AssetId for any avatar that should be displayed with this message.
		/// </summary>
		public string AvatarAssetId { get; set; }

		/// <summary>
		/// The time the message was sent (UTC)
		/// </summary>
		public DateTime SentTimestamp { get; set; }

		/// <summary>
		/// The (optional) time the message will expiry in UTC.
		/// </summary>
		public DateTime? ExpiryTimeStamp { get; set; }

		/// <summary>
		/// The time the message was received (set locally in the SDK)
		/// </summary>
		public DateTime? ReceivedTimestamp { get; set; }

		/// <summary>
		/// The time the message was read (set locally in the SDK)
		/// </summary>
		public DateTime? ReadTimestamp { get; set; }
	}
}