// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DonkyApplication class
//  Author:          Ben Moore
//  Created date:    21/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using Donky.Core.Framework.Logging;
using Donky.Core.Xamarin.Forms.Alerts;
using Xamarin.Forms;

namespace Donky.Core.Xamarin.Forms
{
	/// <summary>
	/// Application class for a Donky-enabled Xamarin Forms app
	/// </summary>
	public class DonkyApplication : Application
	{
		public DonkyApplication()
		{
			AlertManager.Initialise();
		}

		protected override void OnStart()
		{
			Logger.Instance.LogDebug("DonkyApplication.OnStart");
			DonkyCore.Instance.GetService<IAppState>().SetState(true);
		}

		protected override void OnResume()
		{
			Logger.Instance.LogDebug("DonkyApplication.OnResume");
			DonkyCore.Instance.GetService<IAppState>().SetState(true);
		}

		protected override void OnSleep()
		{
			Logger.Instance.LogDebug("DonkyApplication.OnSleep");
			DonkyCore.Instance.GetService<IAppState>().SetState(false);
		}
	}
}