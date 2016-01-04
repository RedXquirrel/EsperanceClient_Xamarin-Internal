// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IRegistrationController interface
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

namespace Donky.Core.Registration
{
	/// <summary>
	/// Public registration operations.
	/// </summary>
	public interface IRegistrationController
	{
		/// <summary>
		/// Gets a value indicating whether the client is registered with the Donky Network.
		/// </summary>
		/// <returns></returns>
		Task<bool> GetIsRegisteredAsync();

		/// <summary>
		/// Gets the details of the current registration.
		/// </summary>
		/// <returns></returns>
		Task<RegistrationDetails> GetRegistrationDetailsAsync();

		/// <summary>
		/// Updates the details associated with the current registration.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="device">The device.</param>
		/// <returns></returns>
		Task<ApiResult> UpdateRegistrationDetailsAsync(UserDetails user = null, DeviceDetails device = null);

        /// <summary>
        /// Replaces the registration asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="device">The device.</param>
        /// <returns></returns>
	    Task ReplaceRegistrationAsync(UserDetails user, DeviceDetails device);

		/// <summary>
		/// Gets the available Tags and their current state.
		/// </summary>
		/// <returns></returns>
		Task<List<TagOption>> GetTagsAsync();

		/// <summary>
		/// Sets the selected tags for the local registration.
		/// </summary>
		/// <param name="tags"></param>
		/// <returns></returns>
		Task<ApiResult> SetTagsAsync(List<TagOption> tags);
	}
}