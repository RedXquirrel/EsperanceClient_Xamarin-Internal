// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DonkyPushLogic class.
//  Author:          Ben Moore
//  Created date:    14/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using Autofac;
using Donky.Core;
using Donky.Core.Framework;
using Donky.Core.Framework.Logging;
using Donky.Core.Framework.Storage;
using Donky.Core.Notifications;
using Donky.Core.Registration;
using Donky.Messaging.Push.Logic.Data;

namespace Donky.Messaging.Push.Logic
{
	/// <summary>
	/// Main entry point for the Donky Push Logic module.
	/// </summary>
	public static class DonkyPushLogic
	{
		public static readonly ModuleDefinition Module = new ModuleDefinition(
			"DonkyDotNetPushLogic", AssemblyHelper.GetAssemblyVersion(typeof(DonkyPushLogic)).ToString());

		private static bool _isInitialised;
 		private static readonly object Lock = new object();

		/// <summary>
		/// Initialises the Donky Push Logic module.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">DonkyPushLogic is already initialised</exception>
		public static void Initialise()
		{
			lock (Lock)
			{
				if (_isInitialised)
				{
					throw new InvalidOperationException("DonkyPushLogic is already initialised");
				}

				DonkyCore.Instance.RegisterModule(Module);
			    DonkyCore.Instance.RegisterServiceType<IPushDataContext, PushDataContext>();

                DonkyCore.Instance.RegisterServiceType<IPushMessagingManager, PushMessagingManager>();

				var notificationManager = DonkyCore.Instance.GetService<INotificationManager>();
				notificationManager.SubscribeToDonkyNotifications(Module,
					new DonkyNotificationSubscription
					{
						AutoAcknowledge = false,
						Type = "SimplePushMessage",
						Handler = n => Instance.HandleSimplePushAsync(n)
					});

				_isInitialised = true;
			}
		}

		/// <summary>
		/// Access to the push messaging logic.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">Module not initialised</exception>
		public static IPushMessagingManager Instance
		{
			get
			{
				if (!_isInitialised)
				{
					throw new InvalidOperationException("Module not initialised");
				}

				return DonkyCore.Instance.GetService<IPushMessagingManager>();
			}
		}
	}
}