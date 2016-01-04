using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

// https://bugzilla.xamarin.com/show_bug.cgi?id=30047

namespace Donky.Messaging.Rich.Inbox.XamarinForms
{
    public class RichMessageWebView : WebView
    {
        public static BindableProperty HtmlStringProperty =
            BindableProperty.Create<RichMessageWebView, string>(ctrl => ctrl.HtmlString,
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var ctrl = (RichMessageWebView)bindable;
                ctrl.HtmlString = newValue;
            });

        private string _htmlString;
        public string HtmlString
        {
            get { return _htmlString; }
            set 
            { 
                _htmlString = value;
                Debug.WriteLine("*** 1 _htmlString: " + _htmlString);
                OnPropertyChanged("HtmlString"); 
            }
        }

        public RichMessageWebView()
        {
            //this.PropertyChanged += (s, e) =>
            //    {
            //        if(e.PropertyName.Equals(RichMessageWebView.HtmlStringProperty.PropertyName))
            //        {
            //            var htmlSource = new HtmlWebViewSource();
            //            htmlSource.Html = HtmlString;

            //            Debug.WriteLine("*** 2 _htmlString: " + htmlSource);

            //            this.Source = htmlSource;
            //        }
            //    };
        }
    }
}
