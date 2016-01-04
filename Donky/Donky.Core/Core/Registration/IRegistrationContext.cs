// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IRegistrationContent interface
//  Author:          Ben Moore
//  Created date:    30/04/2015
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
using System.Collections.Generic;
using System.Threading.Tasks;
using Donky.Core.Notifications.Remote;

namespace Donky.Core.Registration
{
	/// <summary>
	/// Details of the current registration.
	/// </summary>
	public interface IRegistrationContext
	{
        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
		string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the application version.
        /// </summary>
        /// <value>
        /// The application version.
        /// </value>
		string AppVersion { get; set; }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <returns></returns>
		Task<UserDetails> GetUser();

        /// <summary>
        /// Sets the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
		Task SetUser(UserDetails user);

        /// <summary>
        /// Gets the device.
        /// </summary>
        /// <returns></returns>
		Task<DeviceDetails> GetDevice();

        /// <summary>
        /// Sets the device.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns></returns>
		Task SetDevice(DeviceDetails device);

        /// <summary>
        /// Gets the modules.
        /// </summary>
        /// <returns></returns>
		Task<List<ModuleDefinition>> GetModules();

        /// <summary>
        /// Sets the modules.
        /// </summary>
        /// <param name="modules">The modules.</param>
        /// <returns></returns>
		Task SetModules(List<ModuleDefinition> modules);

        /// <summary>
        /// Gets or sets the device secret.
        /// </summary>
        /// <value>
        /// The device secret.
        /// </value>
		string DeviceSecret { get; set; }

        /// <summary>
        /// Gets or sets the network identifier.
        /// </summary>
        /// <value>
        /// The network identifier.
        /// </value>
		string NetworkId { get; set; }

        /// <summary>
        /// Gets the remote channel details.
        /// </summary>
        /// <returns></returns>
		Task<RemoteChannelDetails> GetRemoteChannelDetails();

        /// <summary>
        /// Sets the remote channel details.
        /// </summary>
        /// <param name="details">The details.</param>
        /// <returns></returns>
		Task SetRemoteChannelDetails(RemoteChannelDetails details);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is suspended.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is suspended; otherwise, <c>false</c>.
        /// </value>
		bool IsSuspended { get; set; }
	}
}