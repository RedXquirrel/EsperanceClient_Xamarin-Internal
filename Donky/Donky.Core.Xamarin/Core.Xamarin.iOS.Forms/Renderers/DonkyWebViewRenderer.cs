using Donky.Core.Xamarin.Forms.Views;
using Donky.Core.Xamarin.iOS.Forms.Renderers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRendererAttribute(typeof(DonkyWebView), typeof(DonkyWebViewRenderer))]

namespace Donky.Core.Xamarin.iOS.Forms.Renderers
{
    public class DonkyWebViewRenderer : WebViewRenderer
    {
        public DonkyWebViewRenderer()
        {
            this.ScalesPageToFit = true;
            this.AutoresizingMask = UIKit.UIViewAutoresizing.FlexibleDimensions;
        }
    }
}
