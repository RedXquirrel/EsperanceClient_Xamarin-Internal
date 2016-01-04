// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DonkyGcmBroadcastReceiver class
//  Author:          Ben Moore
//  Created date:    11/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using Android.App;
using Android.Content;

namespace Donky.Core.Xamarin.Android.Gcm
{
	/// <summary>
	/// GCM notification handler
	/// </summary>
	[BroadcastReceiver(Permission = "com.google.android.c2dm.permission.SEND")]
	[IntentFilter(new string[] { "com.google.android.c2dm.intent.RECEIVE" }, Categories = new string[] { "@PACKAGE_NAME@" })]
	[IntentFilter(new string[] { "com.google.android.c2dm.intent.REGISTRATION" }, Categories = new string[] { "@PACKAGE_NAME@" })]
	[IntentFilter(new string[] { "com.google.android.gcm.intent.RETRY" }, Categories = new string[] { "@PACKAGE_NAME@" })]
	public class DonkyGcmBroadcastReceiver : BroadcastReceiver
	{
		const string TAG = "PushHandlerBroadcastReceiver";
		public override void OnReceive(Context context, Intent intent)
		{
			DonkyGcmIntentService.RunIntentInService(context, intent);
			SetResult(Result.Ok, null, null);
		}
	}
}