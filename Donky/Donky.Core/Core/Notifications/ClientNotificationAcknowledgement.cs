// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ClientNotificationAcknowledgement class.
//  Author:          Ben Moore
//  Created date:    07/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
/*
MIT LICENCE:
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE. */
using System;
using System.Collections.Generic;
using Donky.Core.Framework.Extensions;

namespace Donky.Core.Notifications
{
	/// <summary>
	/// Acknowledgement details for a server notification, sent as part of a related client notification.
	/// </summary>
	public class ClientNotificationAcknowledgement : Dictionary<string, object>
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientNotificationAcknowledgement"/> class.
        /// </summary>
		public ClientNotificationAcknowledgement()
		{
		}

		internal ClientNotificationAcknowledgement(Dictionary<string, object> source)
		{
			foreach (var item in source)
			{
				this[item.Key] = item.Value;
			}
		}

        /// <summary>
        /// Gets or sets the server notification identifier.
        /// </summary>
        /// <value>
        /// The server notification identifier.
        /// </value>
		public string ServerNotificationId
		{
			get { return this.ValueOrDefault<string>("serverNotificationId", null); }
			set { this["serverNotificationId"] = value; }
		}

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
		public NotificationResult Result
		{
			get { return this.ValueOrDefault("result", NotificationResult.NoResult); }
			set { this["result"] = value; }
		}

        /// <summary>
        /// Gets or sets the sent time.
        /// </summary>
        /// <value>
        /// The sent time.
        /// </value>
		public DateTime SentTime
		{
			get { return this.ValueOrDefault("sentTime", default(DateTime)); }
			set { this["sentTime"] = value; }
		}

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
		public string Type
		{
			get { return this.ValueOrDefault<string>("type", null); }
			set { this["type"] = value; }
		}

        /// <summary>
        /// Gets or sets the type of the custom notification.
        /// </summary>
        /// <value>
        /// The type of the custom notification.
        /// </value>
		public string CustomNotificationType
		{
			get { return this.ValueOrDefault<string>("customNotificationType", null); }
			set { this["customNotificationType"] = value; }
		}
	}
}