// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     INotificationManager interface
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
using Donky.Core.Registration;

namespace Donky.Core.Notifications
{
	/// <summary>
	/// Additional notification operations for use by Donky consumers only.
	/// </summary>
	public interface INotificationManager : INotificationController, ISubscribeToNotifications
	{
		/// <summary>
		/// Queues the specified client notifications.
		/// </summary>
		/// <param name="notifications">The notifications.</param>
		/// <returns></returns>
		Task QueueClientNotificationAsync(params ClientNotification[] notifications);

		/// <summary>
		/// Sends client notifications.
		/// </summary>
		/// <param name="notifications">The notifications.</param>
		/// <returns></returns>
		Task SendClientNotificationsAsync(params ClientNotification[] notifications);

		/// <summary>
		/// Subscribes to donky notifications.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="subscriptions">The subscriptions.</param>
		void SubscribeToDonkyNotifications(ModuleDefinition module, params DonkyNotificationSubscription[] subscriptions);

		/// <summary>
		/// Gets and processes the server notification with the specified id.
		/// </summary>
		/// <param name="notificationId">The notification identifier.</param>
		/// <returns></returns>
		Task GetAndProcessNotificationAsync(string notificationId);
	}
}