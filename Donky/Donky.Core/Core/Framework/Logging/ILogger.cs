// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ILogger interface.
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
using System.Collections.Generic;
using System.Threading.Tasks;
using Donky.Core.Services;

namespace Donky.Core.Framework.Logging
{
	/// <summary>
	/// Log related operations.
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Logs a debug level message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="args">The arguments.</param>
		void LogDebug(string message, params object[] args);

		/// <summary>
		/// Logs an information level message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="args">The arguments.</param>
		void LogInformation(string message, params object[] args);

		/// <summary>
		/// Logs a warning level message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="args">The arguments.</param>
		void LogWarning(string message, params object[] args);

		/// <summary>
		/// Logs a validation warning.
		/// </summary>
		/// <param name="validationFailures">The validation failures.</param>
		/// <param name="message">The message.</param>
		/// <param name="args">The arguments.</param>
		void LogValidationWarning(IEnumerable<ValidationFailure> validationFailures, string message, params object[] args);

		/// <summary>
		/// Logs an error level message for the specified exception.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="message">The message.</param>
		/// <param name="args">The arguments.</param>
		void LogError(Exception exception, string message, params object[] args);

		/// <summary>
		/// Logs an error level message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="args">The arguments.</param>
		void LogError(string message, params object[] args);

		/// <summary>
		/// Gets the log contents.
		/// </summary>
		/// <returns></returns>
		Task<string> GetLogContentsAsync();
	}
}