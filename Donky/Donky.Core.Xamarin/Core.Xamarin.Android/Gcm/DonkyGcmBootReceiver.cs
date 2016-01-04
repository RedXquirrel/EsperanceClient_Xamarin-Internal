// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DonkyGcmBootReceiver
//  Author:          Ben Moore
//  Created date:    11/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using Android.App;
using Android.Content;

namespace Donky.Core.Xamarin.Android.Gcm
{
	/// <summary>
	/// Ensures the Donky enabled app is running and able to receive notifications on startup
	/// </summary>
	[BroadcastReceiver]
	[IntentFilter(new[] { Intent.ActionBootCompleted })]
	public class DonkyGcmBootReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			DonkyGcmIntentService.RunIntentInService(context, intent);
			SetResult(Result.Ok, null, null);
		}
	}
}