// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IObjectRegistry interface.
//  Author:          Ben Moore
//  Created date:    04/05/2015
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

namespace Donky.Core.Framework.DependencyInjection
{
	/// <summary>
	/// Provides access to the Donky dependency injection framework.
	/// </summary>
	public interface IObjectRegistry
	{
		/// <summary>
		/// Begins a batch of registrations.
		/// </summary>
		void BeginRegistrationBatch();

		/// <summary>
		/// Adds a registration using an existing object instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance">The instance.</param>
		void AddRegistration<T>(T instance) where T : class;

		/// <summary>
		/// Adds a registration using the specified factory function.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="creator">The creator.</param>
		void AddRegistration<T>(Func<T> creator) where T : class;

		/// <summary>
		/// Adds a registration using a type mapping.
		/// </summary>
		/// <typeparam name="TService">The type of the service.</typeparam>
		/// <typeparam name="TInstance">The type of the instance.</typeparam>
		void AddRegistration<TService, TInstance>()
			where TInstance : TService
			where TService : class;

		/// <summary>
		/// Commits a batch of registrations.
		/// </summary>
		void CommitRegistrationBatch();
	}
}