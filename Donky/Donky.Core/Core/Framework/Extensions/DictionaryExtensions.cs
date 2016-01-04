// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DictionaryExtensions class
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
using System.Collections.Generic;

namespace Donky.Core.Framework.Extensions
{
	/// <summary>
	/// Extension methods for dictionaries.
	/// </summary>
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Gets the specified value from the dictionary, or returns the default value if the key is not found.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns></returns>
		public static T ValueOrDefault<T>(this Dictionary<string, object> source, string key, T defaultValue)
		{
			return source.ContainsKey(key)
				? (T) source[key]
				: defaultValue;
		}

		/// <summary>
		/// Returns the specified item from the dictionary as an Dictionary of string/object.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="key">The key.</param>
		/// <remarks>
		/// Useful for navigating generic objects that have been deserialsed from JSON.
		/// </remarks>
		public static Dictionary<string, object> ChildObject(this Dictionary<string, object> source, string key)
		{
			return (Dictionary<string, object>) source[key];
		}

        /// <summary>
        /// Values the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
		public static T Value<T>(this Dictionary<string, object> source, string key)
		{
			return (T) source[key];
		}
	}
}