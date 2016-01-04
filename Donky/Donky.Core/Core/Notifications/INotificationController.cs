// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     INotificationController interface.
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
using System.Threading.Tasks;
using Donky.Core.Notifications.Content;

namespace Donky.Core.Notifications
{
	/// <summary>
	/// Public operations for managing notifications.
	/// </summary>
	public interface INotificationController
	{
		/// <summary>
		/// Synchonises client and server notifications with the Donky Network.
		/// </summary>
		Task<ApiResult> SynchroniseAsync();

		/// <summary>
		/// Queues the specified content notifications.
		/// </summary>
		/// <param name="notifications">The notifications.</param>
		/// <returns></returns>
		Task QueueContentNotificationsAsync(params ContentNotification[] notifications);

		/// <summary>
		/// Sends the specified content notifications.
		/// </summary>
		/// <param name="notifications">The notifications.</param>
		/// <remarks>
		/// Equivalent of a QueueContentNotifications operation followed by a Synchronise.
		/// </remarks>
		Task<ApiResult> SendContentNotificationsAsync(params ContentNotification[] notifications);

		/// <summary>
		/// Gets a specific server notification from the Donky Network.
		/// </summary>
		/// <param name="notificationId">The notification identifier.</param>
		/// <returns></returns>
		Task<ServerNotification> GetServerNotification(string notificationId);
    }
}