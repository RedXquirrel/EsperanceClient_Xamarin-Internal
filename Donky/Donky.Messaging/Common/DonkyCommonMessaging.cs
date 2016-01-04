// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DonkyCommonMessaging module;
//  Author:          Ben Moore
//  Created date:    14/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using Donky.Core;
using Donky.Core.Framework;
using Donky.Core.Registration;

namespace Donky.Messaging.Common
{
	/// <summary>
	/// Module class for Common Messaging functionality
	/// </summary>
	public static class DonkyCommonMessaging
	{
		public static readonly ModuleDefinition Module = new ModuleDefinition(
			"DonkyDotNetCommonMessaging", AssemblyHelper.GetAssemblyVersion(typeof(DonkyCommonMessaging)).ToString());

		/// <summary>
		/// Initialise the Common Messaging module.
		/// </summary>
		public static void Initialise()
		{
			DonkyCore.Instance.RegisterModule(Module);
			DonkyCore.Instance.RegisterServiceType<ICommonMessagingManager, CommonMessagingManager>();
		}
	}
}