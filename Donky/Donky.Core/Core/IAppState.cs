// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IAppState interface.
//  Author:          Ben Moore
//  Created date:    21/05/2015
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
namespace Donky.Core
{
	/// <summary>
	/// Defines the current state of the App.
	/// </summary>
	public interface IAppState
	{
		/// <summary>
		/// Returns true if the app is open.
		/// </summary>
		bool IsOpen { get; }

		/// <summary>
		/// Indicates whether the app was opened from a notification.
		/// </summary>
		bool WasOpenedFromNotification { get; set; }

		/// <summary>
		/// The id of the notification that was used to launch the app.
		/// </summary>
		string LaunchingNotificationId { get; set; }

		/// <summary>
		/// Sets the current state.
		/// </summary>
		/// <param name="isOpen">if set to <c>true</c> the app is open.</param>
		void SetState(bool isOpen);
	}
}