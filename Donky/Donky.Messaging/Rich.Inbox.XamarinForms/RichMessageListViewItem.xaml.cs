using Donky.Messaging.Rich.Inbox.XamarinForms.Converters;
using Donky.Messaging.Rich.Inbox.XamarinForms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Donky.Messaging.Rich.Inbox.XamarinForms
{
    public partial class RichMessageInboxListViewItem : ContentView
    {

        public static BindableProperty RichMessageInboxListViewItemViewModelProperty =
            BindableProperty.Create<RichMessageInboxListViewItem, RichMessageInboxListViewItemViewModel>(
            ctrl => ctrl.RichMessageInboxListViewItemViewModel,
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var ctrl = (RichMessageInboxListViewItem)bindable;
                ctrl.RichMessageInboxListViewItemViewModel = newValue;
            });

        private RichMessageInboxListViewItemViewModel _richMessageInboxListViewItemViewModel;
        public RichMessageInboxListViewItemViewModel RichMessageInboxListViewItemViewModel
        {
            get { return _richMessageInboxListViewItemViewModel; }
            set { _richMessageInboxListViewItemViewModel = value; OnPropertyChanged("RichMessageInboxListViewItemViewModel"); }
        }

        private bool _isViewModelPropertyChangedSubscribed;
        public RichMessageInboxListViewItem()
        {
            InitializeComponent();

            this.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName.Equals("RichMessageInboxListViewItemViewModel"))
                    {
                        if(this.RichMessageInboxListViewItemViewModel != null)
                        {
                            if (!_isViewModelPropertyChangedSubscribed)
                            {
                                this.RichMessageInboxListViewItemViewModel.PropertyChanged += RichMessageInboxListViewItemViewModel_PropertyChanged;
                                _isViewModelPropertyChangedSubscribed = true;
                            }
                        }
                        else
                        {
                            if (_isViewModelPropertyChangedSubscribed)
                            {
                                this.RichMessageInboxListViewItemViewModel.PropertyChanged -= RichMessageInboxListViewItemViewModel_PropertyChanged;
                                _isViewModelPropertyChangedSubscribed = false;
                            }
                        }


                        if(this.testLabel != null && this.RichMessageInboxListViewItemViewModel != null)
                        {
                            this.testLabel.Text = this.RichMessageInboxListViewItemViewModel.RichMessage.SenderDisplayName;
                        }

                        var colourConverter = new BooleanToColourConverter();

                        this.BackgroundColor = (Color)colourConverter.Convert(this.RichMessageInboxListViewItemViewModel.ShowItemMultiSelector, null, "#ff0000:#0000ff", null);

                    }
                };
        }

        void RichMessageInboxListViewItemViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var colourConverter = new BooleanToColourConverter();

            this.BackgroundColor = (Color)colourConverter.Convert(this.RichMessageInboxListViewItemViewModel.ShowItemMultiSelector, null, "#ff0000:#0000ff", null);
        }
    }
}
