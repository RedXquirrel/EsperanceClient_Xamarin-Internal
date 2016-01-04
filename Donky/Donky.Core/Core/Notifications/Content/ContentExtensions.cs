// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ContentExtensions class.
//  Author:          Ben Moore
//  Created date:    08/05/2015
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
using System.Linq;

namespace Donky.Core.Notifications.Content
{
	/// <summary>
	/// Extension methods to allow fluent building of ContentNotifications. 
	/// </summary>
	public static class ContentExtensions
	{
		/// <summary>
		/// Adds a SpecifiedUsers audience definition with the specified users.
		/// </summary>
		/// <param name="notification">The notification.</param>
		/// <param name="userIds">The user ids.</param>
		/// <returns></returns>
		public static ContentNotification ForUsers(this ContentNotification notification, params string[] userIds)
		{
			notification.Audience = new SpecifiedUsersAudience
			{
				Users = userIds.Select(x => new AudienceMember
				{
					UserId = x
				}).ToList()
			};

			return notification;
		}

		/// <summary>
		/// Adds custom content to the ContentNotification.
		/// </summary>
		/// <param name="notification">The notification.</param>
		/// <param name="customType">The custom content type.</param>
		/// <param name="data">The custom data.</param>
		/// <returns></returns>
		public static ContentNotification WithCustomContent(this ContentNotification notification,
			string customType, object data)
		{
			notification.Content = new CustomNotificationContent
			{
				CustomType = customType,
				Data = data
			};

			return notification;
		}
	}
}