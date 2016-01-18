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

            AppBootstrap.LoggedIn = false;

            DonkyiOSForms.Init();         // Xamarin Forms Custom iOS Renderers
            ImageCircleRenderer.Init();
            DonkyiOS.Initialise();        // Xamarin.iOS framework
            DonkyPushUIiOS.Initialise();  // Xamarin.iOS framework
            AppBootstrap.Initialise();    // Xamarin.Forms PCL

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}

        public override void RegisteredForRemoteNotifications(
            UIApplication application,
            NSData deviceToken)
        {
            DonkyiOS.RegisteredForRemoteNotifications(application, deviceToken);
        }

        public override void DidReceiveRemoteNotification(
          UIApplication application,
          NSDictionary userInfo,
          Action<UIBackgroundFetchResult> completionHandler)
        {
            DonkyiOS.DidReceiveRemoteNotification(
              application,
              userInfo,
              completionHandler);
        }

        public override void HandleAction(
          UIApplication application,
          string actionIdentifier,
          NSDictionary remoteNotificationInfo,
          Action completionHandler)
        {
            DonkyiOS.HandleAction(
              application,
              actionIdentifier,
              remoteNotificationInfo,
              completionHandler);
        }

        public override void FailedToRegisterForRemoteNotifications(
          UIApplication application,
          NSError error)
        {
            DonkyiOS.FailedToRegisterForRemoteNotifications(application, error);
        }
	}
}

