using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Donky.Messaging.Rich.Inbox.Android;
using Donky.Messaging.Rich.Inbox.XamarinForms;
using Android.Webkit;

[assembly: ExportRenderer(typeof(RichMessageWebView), typeof(RichMessageWebViewRenderer_Android))]

namespace Donky.Messaging.Rich.Inbox.Android
{
    [Preserve]
    public class RichMessageWebViewRenderer_Android : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if(Control != null)
            {

            }
        }
    }
}