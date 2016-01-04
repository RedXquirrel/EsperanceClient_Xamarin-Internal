// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     TaskExtensions class
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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Donky.Core.Framework.Logging;

namespace Donky.Core.Framework.Extensions
{
	/// <summary>
	/// Extension methods for tasks.
	/// </summary>
	public static class TaskExtensions
	{
        public static ILogger Logger { get; set; }
		/// <summary>
		/// Runs the task in the background without awaiting the response.  Exceptions are logged.
		/// </summary>
		/// <param name="task">The task.</param>
		public static void ExecuteInBackground(this Task task)
		{
			Task.Run(() => task)
				.ContinueWith(
					t => Logger.LogError(t.Exception, "Background task threw exception"),
					TaskContinuationOptions.OnlyOnFaulted);
		}
		
		/// <summary>
		/// Runs a task synchronously - for use in non-async scenarios.
		/// </summary>
		/// <param name="task">The task.</param>
		public static T ExecuteSynchronously<T>(this Task<T> task)
		{
			return Task.Run(() => task).Result;
		}

        /// <summary>
        /// Executes the Task synchronously.
        /// </summary>
        /// <param name="task">The task.</param>
		public static void ExecuteSynchronously(this Task task)
		{
			Task.Run(() => task).Wait();
		}
	}
}