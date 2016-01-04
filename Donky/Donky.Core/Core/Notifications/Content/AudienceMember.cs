// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     AudienceMember class.
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
using System.Collections.Generic;

namespace Donky.Core.Notifications.Content
{
	/// <summary>
	/// Defines a member of an explicit audience.
	/// </summary>
	public class AudienceMember
	{
		/// <summary>
		/// The user id.
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// The correlation id for responses from this user.
		/// </summary>
		public string CorrelationId { get; set; }

		/// <summary>
		/// Additional data that can be used by the Donky Network when templating / personalising content.
		/// </summary>
		public Dictionary<string, string> TemplateData { get; set; }
	}
}