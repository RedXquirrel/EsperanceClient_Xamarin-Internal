// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IConfigurationManager operations.
//  Author:          Ben Moore
//  Created date:    14/05/2015
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
using Newtonsoft.Json.Linq;

namespace Donky.Core.Configuration
{
	/// <summary>
	/// Provides access to configuration related operations.
	/// </summary>
	public interface IConfigurationManager
	{
		/// <summary>
		/// Initialise the configuration manager.
		/// </summary>
		/// <returns></returns>
		Task InitialiseAsync();

		/// <summary>
		/// Update all stored configuration values and custom sets.
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="configurationSets">The configuration sets.</param>
		/// <returns></returns>
		Task UpdateConfigurationAsync(Dictionary<string, string> values,
			Dictionary<string, JObject> configurationSets);

		/// <summary>
		/// Store the supplied values in configuration.
		/// </summary>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		Task SetValuesAsync(Dictionary<string, string> values);

		/// <summary>
		/// Stores the specifed value in the configuration store.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		Task SetValueAsync(string key, string value);

		/// <summary>
		/// Gets the specified configuration value.
		/// </summary>
		/// <typeparam name="T">The type of the configuration item.</typeparam>
		/// <param name="key">The key for the configuration item.</param>
		/// <returns>The requested configuration item, or default(T) if not found.</returns>
		T GetValue<T>(string key);
	}
}