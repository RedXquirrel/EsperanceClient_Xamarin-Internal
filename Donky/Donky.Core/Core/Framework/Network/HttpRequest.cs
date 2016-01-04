// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     HttpRequest class.
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
using System.Collections.Generic;
using System.Net.Http;

namespace Donky.Core.Framework.Network
{
	/// <summary>
	/// Represents an HTTP request with the specified type of body.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class HttpRequest<T>
	{
		/// <summary>
		/// The target URI.
		/// </summary>
		public string Uri { get; set; }

		/// <summary>
		/// The request body.
		/// </summary>
		public T Body { get; set; }

		/// <summary>
		/// The HTTP method to use.
		/// </summary>
		public HttpMethod Method { get; set; }

		/// <summary>
		/// The HTTP headers to send with the request.
		/// </summary>
		public Dictionary<string, string> Headers { get; set; }
	}
}