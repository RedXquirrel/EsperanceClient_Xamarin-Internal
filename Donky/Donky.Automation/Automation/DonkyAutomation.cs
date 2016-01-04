// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     Module definition for Donky Automation Logic
//  Author:          Ben Moore
//  Created date:    21/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using Donky.Core;
using Donky.Core.Framework;
using Donky.Core.Registration;

namespace Donky.Automation
{
	/// <summary>
	/// Module definition for Donky Automation Logic
	/// </summary>
	public static class DonkyAutomation
	{
		public static readonly ModuleDefinition Module = new ModuleDefinition(
			"DonkyDotNetAutomation", AssemblyHelper.GetAssemblyVersion(typeof(DonkyAutomation)).ToString());

		private static bool _isInitialised;
		private static readonly object Lock = new object();

		/// <summary>
		/// Initialise the module..
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
				DonkyCore.Instance.RegisterServiceType<IAutomationManager, AutomationManager>();

				_isInitialised = true;
			}
		}

		/// <summary>
		/// Provides static access to automation logic.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">Module is not initialised</exception>
		public static IAutomationManager AutomationManager
		{
			get
			{
				if (!_isInitialised)
				{
					throw new InvalidOperationException("Module is not initialised");
				}

				return DonkyCore.Instance.GetService<IAutomationManager>();
			}
		}
	}
}