// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IDonkyCore interface.
//  Author:          Ben Moore
//  Created date:    29/04/2015
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
using System.Threading.Tasks;
using Donky.Core.Diagnostics;
using Donky.Core.Events;
using Donky.Core.Notifications;
using Donky.Core.Registration;

namespace Donky.Core
{
	/// <summary>
	/// Core Donky APIs
	/// </summary>
	public interface IDonkyCore : ISubscribeToNotifications
	{
		/// <summary>
		/// Initialises the SDK.
		/// </summary>
		/// <param name="apiKey">The API key.</param>
		/// <param name="user">The user.</param>
		/// <param name="device">The device.</param>
		/// <param name="appVersion">The app version.</param>
		/// <param name="publicServiceBaseUrl">The public service base URL.</param>
		/// <returns></returns>
		Task<ApiResult> InitialiseAsync(string apiKey, UserDetails user = null, DeviceDetails device = null, string appVersion = null, string publicServiceBaseUrl = null, string notificationSoundFilename = null);

		/// <summary>
		/// The registration controller.
		/// </summary>
		IRegistrationController RegistrationController { get; }

		/// <summary>
		/// The notification controller.
		/// </summary>
		INotificationController NotificationController { get; }

		/// <summary>
		/// The logging controller.
		/// </summary>
		ILoggingController LoggingController { get; }

		/// <summary>
		/// Registers a module with the SDK.
		/// </summary>
		/// <param name="definition">The definition.</param>
		void RegisterModule(ModuleDefinition definition);

		/// <summary>
		/// Registers a service instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance">The instance.</param>
		void RegisterService<T>(T instance) where T : class;

		/// <summary>
		/// Registers a type mapping for a service.
		/// </summary>
		/// <typeparam name="TService">The type of the service.</typeparam>
		/// <typeparam name="TInstance">The type of the instance.</typeparam>
		void RegisterServiceType<TService, TInstance>() 
			where TInstance : TService 
			where TService : class;

		/// <summary>
		/// Gets the registered implementation of the specified service.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T GetService<T>() where T : class;

		/// <summary>
		/// Subscribes to local events of the specified type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="handler">The handler.</param>
		void SubscribeToLocalEvent<T>(Action<T> handler) where T : LocalEvent;

		/// <summary>
		/// Unsubscribes from local events of the specified type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="handler">The handler.</param>
		void UnsubscribeFromLocalEvent<T>(Action<T> handler) where T : LocalEvent;

		/// <summary>
		/// Publishes a local event.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="theEvent">The event.</param>
		/// <param name="publisher">The publisher.</param>
		void PublishLocalEvent<T>(T theEvent, ModuleDefinition publisher) where T : LocalEvent;

		/// <summary>
		/// Gets a value indicating whether the SDK is initialised.
		/// </summary>
		bool IsInitialised { get; }
    }
}