// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IPersistentStorage interface.
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
using System.Threading.Tasks;

namespace Donky.Core.Framework.Storage
{
	/// <summary>
	/// Interface for basic access to persistent storage
	/// </summary>
	public interface IPersistentStorage
	{
		/// <summary>
		/// Sets the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		void Set(string key, string value);

		/// <summary>
		/// Gets the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		string Get(string key);

		/// <summary>
		/// Saves an object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id">The identifier.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		Task SaveObjectAsync<T>(string id, T value) where T : class;

		/// <summary>
		/// Loads an object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		Task<T> LoadObjectAsync<T>(string id) where T : class;
	}
}