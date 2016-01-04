// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DonkyXamarinForms class
//  Author:          Ben Moore
//  Created date:    21/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using Donky.Core.Framework;
using Donky.Core.Registration;

namespace Donky.Core.Xamarin.Forms
{
	/// <summary>
	/// Donky Xamarin Forms module
	/// </summary>
	public static class DonkyXamarinForms
	{
		public static readonly ModuleDefinition Module = new ModuleDefinition(
			"DonkyXamarinFormsCore", AssemblyHelper.GetAssemblyVersion(typeof(DonkyXamarinForms)).ToString());

		/// <summary>
		/// Initialises this module.
		/// </summary>
		public static void Initialise()
		{
			DonkyCore.Instance.RegisterModule(Module);
		}
	}
}