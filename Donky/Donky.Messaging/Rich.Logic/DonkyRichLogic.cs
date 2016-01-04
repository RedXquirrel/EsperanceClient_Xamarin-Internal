// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DonkyRichLogic module definition.
//  Author:          Ben Moore
//  Created date:    06/07/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using Donky.Core;
using Donky.Core.Framework;
using Donky.Core.Notifications;
using Donky.Core.Registration;
using Donky.Messaging.Rich.Logic.Data;

namespace Donky.Messaging.Rich.Logic
{
	/// <summary>
	/// Main entry point for the Donky Rich Logic module.
	/// </summary>
	public static class DonkyRichLogic
	{
		public static readonly ModuleDefinition Module = new ModuleDefinition(
			"DonkyDotNetRichLogic", AssemblyHelper.GetAssemblyVersion(typeof(DonkyRichLogic)).ToString());

		private static bool _isInitialised;
 		private static readonly object Lock = new object();

		/// <summary>
		/// Initialises the Donky Rich Logic module.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">DonkyRichLogic is already initialised</exception>
		public static void Initialise()
		{
			lock (Lock)
			{
				if (_isInitialised)
				{
					throw new InvalidOperationException("DonkyRichLogic is already initialised");
				}

				DonkyCore.Instance.RegisterModule(Module);
				DonkyCore.Instance.RegisterServiceType<IRichDataContext, RichDataContext>();
				DonkyCore.Instance.RegisterServiceType<IRichMessagingManager, RichMessagingManager>();

				var notificationManager = DonkyCore.Instance.GetService<INotificationManager>();
				notificationManager.SubscribeToDonkyNotifications(Module,
					new DonkyNotificationSubscription
					{
						AutoAcknowledge = false,
						Type = "RichMessage",
						Handler = n => Instance.HandleRichMessageAsync(n)
					});

				_isInitialised = true;
			}
		}

		/// <summary>
		/// Access to the rich messaging logic.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">Module not initialised</exception>
		public static IRichMessagingManager Instance
		{
			get
			{
				if (!_isInitialised)
				{
					throw new InvalidOperationException("Module not initialised");
				}

				return DonkyCore.Instance.GetService<IRichMessagingManager>();
			}
		}
	}
}