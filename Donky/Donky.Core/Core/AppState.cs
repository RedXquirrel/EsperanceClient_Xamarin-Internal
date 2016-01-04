// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     AppState class
//  Author:          Ben Moore
//  Created date:    22/05/2015
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
using Donky.Core.Events;
using Donky.Core.Framework.Events;
using Donky.Core.Framework.Extensions;

namespace Donky.Core
{
	internal class AppState : IAppState
	{
		private readonly IEventBus _eventBus;
		private readonly object _lock = new object();

		internal AppState(IEventBus eventBus)
		{
			_eventBus = eventBus;
		}

		public bool IsOpen { get; private set; }

		public bool WasOpenedFromNotification { get; set; }
		
		public string LaunchingNotificationId { get; set; }

		public void SetState(bool isOpen)
		{
			if (isOpen != IsOpen)
			{
				lock (_lock)
				{
					if (isOpen != IsOpen)
					{
						SetStateInternal(isOpen);
					}
				}
			}
		}

		private void SetStateInternal(bool isOpen)
		{
			IsOpen = isOpen;

			if (isOpen)
			{
				_eventBus.PublishAsync(new AppOpenEvent
				{
					WasLaunchedFromNotification = WasOpenedFromNotification,
					Publisher = DonkyCoreImplementation.CoreModuleDefinition
				}).ExecuteInBackground();
			}
			else
			{
				WasOpenedFromNotification = false;
				LaunchingNotificationId = null;

				_eventBus.PublishAsync(new AppCloseEvent
				{
					Publisher = DonkyCoreImplementation.CoreModuleDefinition
				}).ExecuteInBackground();
			}
		}
	}
}