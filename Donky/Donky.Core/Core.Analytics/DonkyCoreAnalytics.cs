// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DonkyCoreAnalytics module definition.
//  Author:          Ben Moore
//  Created date:    21/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using Donky.Core.Events;
using Donky.Core.Framework;
using Donky.Core.Registration;

namespace Donky.Core.Analytics
{
	/// <summary>
	/// Core Analytics module
	/// </summary>
	public static class DonkyCoreAnalytics
	{
		public static readonly ModuleDefinition Module = new ModuleDefinition(
			"DonkyDotNetCoreAnalytics", AssemblyHelper.GetAssemblyVersion(typeof(DonkyCoreAnalytics)).ToString());

		private static bool _isInitialised;
		private static readonly object Lock = new object();

		/// <summary>
		/// Initialises the Core Analytics module.
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
				DonkyCore.Instance.RegisterServiceType<IAnalyticsManager, AnalyticsManager>();

				_isInitialised = true;

				DonkyCore.Instance.SubscribeToLocalEvent<AppOpenEvent>(AnalyticsManager.HandleAppOpen);
				DonkyCore.Instance.SubscribeToLocalEvent<AppCloseEvent>(AnalyticsManager.HandleAppClose);
			}
		}

		public static IAnalyticsManager AnalyticsManager
		{
			get
			{
				if (!_isInitialised)
				{
					throw new InvalidOperationException("Module is not initialised");
				}

				return DonkyCore.Instance.GetService<IAnalyticsManager>();
			}
		}
	}
}