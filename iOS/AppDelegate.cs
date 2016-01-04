using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Donky.Core.Xamarin.iOS.Forms;
using ImageCircle.Forms.Plugin.iOS;
using Donky.Core.Xamarin.iOS;
using Donky.Messaging.Push.UI.iOS;

namespace EsperanceClient.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

            DonkyiOSForms.Init();         // Xamarin Forms Custom iOS Renderers
            ImageCircleRenderer.Init();
            DonkyiOS.Initialise();        // Xamarin.iOS framework
            DonkyPushUIiOS.Initialise();  // Xamarin.iOS framework
            AppBootstrap.Initialise();    // Xamarin.Forms PCL

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

