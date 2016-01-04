// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     RichMessage entity.
//  Author:          Ben Moore
//  Created date:    03/07/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using Donky.Messaging.Common;

namespace Donky.Messaging.Rich.Logic
{
	/// <summary>
	/// Represents a Donky Rich Message
	/// </summary>
	public class RichMessage : EnhancedMessage
	{
		/// <summary>
		/// The message description
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Optional expired content
		/// </summary>
		public string ExpiredBody { get; set; }
		
		/// <summary>
		/// Indicates whether the message can be forwarded.
		/// </summary>
		public bool CanForward { get; set; }

		/// <summary>
		/// Indicates whether the message can be shared.
		/// </summary>
		public bool CanShare { get; set; }

		/// <summary>
		/// Gets or sets the URL to use when sharing
		/// </summary>
		public string UrlToShare { get; set; }
		
		/// <summary>
		/// details of the message forwarder (if the message was forwarded)
		/// </summary>
		public ForwarderInformation ForwardedBy { get; set; }

		/// <summary>
		/// The overlay message (if the message was forwarded)
		/// </summary>
		public string ForwardingOverlayMessage { get; set; }
	}
}