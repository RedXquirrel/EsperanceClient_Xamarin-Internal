// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ResourceLock class.
//  Author:          Ben Moore
//  Created date:    29/07/2015
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

using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Donky.Core.Framework
{
	/// <summary>
	/// Provides locking around a named resource.
	/// </summary>
	public static class ResourceLock
	{
		private static readonly ConcurrentDictionary<string, SemaphoreSlim> Semaphores = new ConcurrentDictionary<string, SemaphoreSlim>();

		/// <summary>
		/// Wait for exclusive access to the resource.
		/// </summary>
		/// <param name="resourceId">The identifier for the resource you wish to access.</param>
		/// <returns></returns>
		public static async Task WaitAsync(string resourceId)
		{
			var semaphore = Semaphores.GetOrAdd(resourceId, id => new SemaphoreSlim(1));

			await semaphore.WaitAsync();
		}

		/// <summary>
		/// Wait for exclusive access to the resource.
		/// </summary>
		/// <param name="resourceId">The identifier for the resource you wish to access.</param>
		/// <returns></returns>
		public static void Wait(string resourceId)
		{
			var semaphore = Semaphores.GetOrAdd(resourceId, id => new SemaphoreSlim(1));

			semaphore.Wait();
		}

		/// <summary>
		/// Release access to the resource.
		/// </summary>
		/// <param name="resourceId"></param>
		public static void Release(string resourceId)
		{
			SemaphoreSlim semaphore;
			if (Semaphores.TryGetValue(resourceId, out semaphore))
			{
				semaphore.Release();
			}
		}
	}
}