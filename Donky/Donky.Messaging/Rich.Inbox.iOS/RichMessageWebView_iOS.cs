using Donky.Messaging.Rich.Inbox.iOS;
using Donky.Messaging.Rich.Inbox.XamarinForms;
using Foundation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RichMessageWebView), typeof(RichMessageWebViewRenderer))]

namespace Donky.Messaging.Rich.Inbox.iOS
{
    public class RichMessageWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            Debug.WriteLine("*** ios renderer");

            var webview = this;
            webview.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
            webview.ScalesPageToFit;
        }
    }
}
