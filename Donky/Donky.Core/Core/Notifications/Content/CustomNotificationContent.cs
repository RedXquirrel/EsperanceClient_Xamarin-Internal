// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     CustomNotificationContent class.
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
namespace Donky.Core.Notifications.Content
{
	/// <summary>
	/// Content definition for sending Custom data.
	/// </summary>
	public class CustomNotificationContent : Content
	{
		/// <summary>
		/// The type of the content.
		/// </summary>
		public override string Type { get { return "Custom"; } }

		/// <summary>
		/// The type of custom data.
		/// </summary>
		public string CustomType { get; set; }

		/// <summary>
		/// The data to send (will be serialised to JSON for transmission).
		/// </summary>
		public object Data { get; set; }
	}
}