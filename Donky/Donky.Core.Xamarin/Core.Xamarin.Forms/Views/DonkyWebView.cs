using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Donky.Core.Xamarin.Forms.Views
{
    public class DonkyWebView : WebView
    {
        public static readonly BindableProperty HtmlStringProperty =
            BindableProperty.Create<DonkyWebView, string>(ctrl => ctrl.HtmlString,
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var ctrl = (DonkyWebView)bindable;
                ctrl.HtmlString = newValue;
            });

            public string HtmlString
            {
                get { return (string)base.GetValue(HtmlStringProperty); }
                set { base.SetValue(HtmlStringProperty, value); }
            }

        public DonkyWebView()
        {
            this.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName.Equals(DonkyWebView.HtmlStringProperty.PropertyName))
                    {
                        var htmlSource = new HtmlWebViewSource();
                        htmlSource.Html = HtmlString;

                        this.Source = htmlSource;
                    }
                };
        }

    }
}
