using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Donky.Core.Xamarin.Android;

namespace EsperanceClient.Droid
{
	[Activity (Label = "EsperanceClient.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            // Ensure Donky knows if we were launched from a notification
            DonkyAndroid.ActivityLaunchedWithIntent(this);

			global::Xamarin.Forms.Forms.Init (this, bundle);

            LoadApplication (new App());
		}
	}
}

