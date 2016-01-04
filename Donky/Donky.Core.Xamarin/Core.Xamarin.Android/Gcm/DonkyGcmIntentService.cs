// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DonkyGcmIntentService class
//  Author:          Ben Moore
//  Created date:    11/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using Android.App;
using Android.Content;
using Android.OS;
using Donky.Core.Framework.Logging;
using Donky.Core.Notifications.Remote;

namespace Donky.Core.Xamarin.Android.Gcm
{
	/// <summary>
	/// IntentService for handling GCM
	/// </summary>
	[Service]
	public class DonkyGcmIntentService : IntentService
	{
		private static PowerManager.WakeLock _wakeLock;
		private static readonly object _lock = new object();

		internal static void RunIntentInService(Context context, Intent intent)
		{
			lock (_lock)
			{
				if (_wakeLock == null)
				{
					// This is called from BroadcastReceiver, there is no init.
					var pm = PowerManager.FromContext(context);
					_wakeLock = pm.NewWakeLock(
						WakeLockFlags.Partial, "Donky GCM Wakelock");
				}
			}

			_wakeLock.Acquire();
			intent.SetClass(context, typeof(DonkyGcmIntentService));
			context.StartService(intent);
		}

		protected override void OnHandleIntent(Intent intent)
		{
			try
			{
				Context context = this.ApplicationContext;
				string action = intent.Action;

				if (action.Equals("com.google.android.c2dm.intent.REGISTRATION"))
				{
					HandleRegistration(context, intent);
				}
				else if (action.Equals("com.google.android.c2dm.intent.RECEIVE"))
				{
					HandleMessage(context, intent);
				}
			}
			finally
			{
				lock (_lock)
				{
					//Sanity check for null as this is a public method
					if (_wakeLock != null)
						_wakeLock.Release();
				}
			}
		}

		private void HandleRegistration(Context context, Intent intent)
		{
			var token = intent.GetStringExtra("registration_id");
			Logger.Instance.LogInformation("Got GCM Token {0}", token);
			DonkyCore.Instance.PublishLocalEvent(new RemoteChannelDetailsChanged
				{
					Details = new RemoteChannelDetails {Token = token},
				},
				DonkyAndroid.Module);
		}

		private void HandleMessage(Context context, Intent intent)
		{
			DonkyCore.Instance.PublishLocalEvent(new GcmNotificationReceivedEvent
			{
				Intent = intent
			}, DonkyAndroid.Module);
		}
	}
}