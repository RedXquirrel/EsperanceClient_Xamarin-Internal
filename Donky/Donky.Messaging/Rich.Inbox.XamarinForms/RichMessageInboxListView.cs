using Donky.Messaging.Rich.Inbox.XamarinForms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Donky.Messaging.Rich.Inbox.XamarinForms
{
    public class RichMessageInboxListView : ListView
    {
        public static BindableProperty ShowItemMultiSelectorProperty =
        BindableProperty.Create<RichMessageInboxListView, bool>(ctrl => ctrl.ShowItemMultiSelector,
        defaultValue: false,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var ctrl = (RichMessageInboxListView)bindable;
            ctrl.ShowItemMultiSelector = newValue;
        });

        private bool _showItemMultiSelector;
        public bool ShowItemMultiSelector
        {
            get { return _showItemMultiSelector; }
            set { _showItemMultiSelector = value; OnPropertyChanged("ShowItemMultiSelector"); }
        }

        public RichMessageInboxListView()
        {
            this.PropertyChanged += (s, e) =>
                {
                    if(e.PropertyName.Equals("ShowItemMultiSelector"))
                    {
                        foreach (var item in this.ItemsSource)
                        {
                            ((RichMessageInboxListViewItemViewModel)item).ShowItemMultiSelector = ShowItemMultiSelector;
                        }
                    }

                };
        }
    }
}
