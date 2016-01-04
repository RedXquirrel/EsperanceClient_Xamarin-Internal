// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     EventBus class
//  Author:          Ben Moore
//  Created date:    13/05/2015
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
using System.Linq;
using System.Threading.Tasks;
using Donky.Core.Events;
using Donky.Core.Framework.Logging;

namespace Donky.Core.Framework.Events
{
	/// <summary>
	/// Implementation of the IEventBus interface.
	/// </summary>
	internal class EventBus : IEventBus
	{
		// Tuple: Type, Handler(wrapped), Handler(direct ref)
		private readonly List<Tuple<Type, Action<object>, object>> _subscriptions = new List<Tuple<Type, Action<object>, object>>();
		private readonly object _subscriptionLock = new object();
 
		public async Task PublishAsync<T>(T theEvent) where T : LocalEvent
		{
			var subscribers = _subscriptions.Where(s => s.Item1 == typeof (T))
				.Select(s => s.Item2)
				.ToList();

			if (!theEvent.IsLoggingEvent)
			{
				// Don't log logging events otherwise we get a stack overflow!
				Logger.Instance.LogDebug("Publishing event of type {0} to {1} subscribers",
					typeof (T).Name, subscribers.Count);
			}

			if (subscribers.Count > 0)
			{
				await NotifySubscribers(theEvent, subscribers);
			}
		}

		public void Subscribe<T>(Action<T> handler) where T : LocalEvent
		{
			lock (_subscriptionLock)
			{
				_subscriptions.Add(new Tuple<Type, Action<object>, object>(typeof(T), o => handler(o as T), handler));
			}
		}

		public void Unsubscribe<T>(Action<T> handler) where T : LocalEvent
		{
			lock (_subscriptionLock)
			{
				var toRemove = _subscriptions.Where(s => ReferenceEquals(s.Item3, handler));
				foreach (var subscription in toRemove)
				{
					_subscriptions.Remove(subscription);
				}
			}
		}

		private Task NotifySubscribers(object theEvent, IEnumerable<Action<object>> subscribers)
		{
			var tasks = subscribers.Select(s => Task.Run(() => s(theEvent)));

			return Task.WhenAll(tasks);
		}
	}
}