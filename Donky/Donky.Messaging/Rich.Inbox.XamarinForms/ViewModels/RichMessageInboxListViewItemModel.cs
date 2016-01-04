using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Donky.Messaging.Rich.Logic;
using System.Windows.Input;
using Donky.Core;
using Donky.Core.Assets;
using Xamarin.Forms;
using System.Diagnostics;

namespace Donky.Messaging.Rich.Inbox.XamarinForms.ViewModels
{
    public class RichMessageInboxListViewItemViewModel : ViewModelBase
    {
        // EllipsesCommand
        private ICommand _selectCommand;
        public ICommand SelectCommand
        {
            get
            {
                return _selectCommand;
            }
            set
            {
                _selectCommand = value;
                RaisePropertyChanged("SelectCommand");
            }
        }

        private ICommand _ellipsesCommand;
        public ICommand EllipsesCommand
        {
            get
            {
                return _ellipsesCommand;
            }
            set
            {
                _ellipsesCommand = value;
                RaisePropertyChanged("EllipsesCommand");
            }
        }

        private ICommand _forwardCommand;
        public ICommand ForwardCommand
        {
            get
            {
                return _forwardCommand;
            }
            set
            {
                _forwardCommand = value;
                RaisePropertyChanged("ForwardCommand");
            }
        }

        private ICommand _shareCommand;
        public ICommand ShareCommand
        {
            get
            {
                return _shareCommand;
            }
            set
            {
                _shareCommand = value;
                RaisePropertyChanged("ShareCommand");
            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand;
            }
            set
            {
                _deleteCommand = value;
                RaisePropertyChanged("DeleteCommand");
            }
        }

        private RichMessage _richMessage;
        public RichMessage RichMessage
        {
            get
            {
                return _richMessage;
            }
            set
            {
                _richMessage = value;
                if (_richMessage == null)
                {
                    AvatarUri = null;
                }
                else
                {
                    //var assetHelper = DonkyCore.Instance.GetService<IAssetHelper>();
                    //alertView.Image.Source = new UriImageSource
                    //{
                    //    Uri = new Uri(assetHelper.CreateUriForAsset(avatarId))
                    //};
                    var auri = DonkyCore.Instance.GetService<IAssetHelper>().CreateUriForAsset(RichMessage.AvatarAssetId);
                    if(string.IsNullOrEmpty(auri))
                    {
                        //AvatarUri = new Uri("https://dev-client-api.mobiledonky.com/asset/094867f5-24c4-48e0-88e1-1a53d52ca3de%7C20150924%7CNE_DEV_RC1_RG1");
                        AvatarUri = new Uri(auri);
                    }
                    else
                    {
                        AvatarUri = new Uri(auri);
                    }
                }

                RaisePropertyChanged("RichMessage");
                RaisePropertyChanged("ShareMenuItemText");
            }
        }

        private bool _showItemMultiSelector;
        public bool ShowItemMultiSelector
        {
            get
            {
                return _showItemMultiSelector;
            }
            set
            {
                _showItemMultiSelector = value;
                RaisePropertyChanged("SelectedMultiselect");
                RaisePropertyChanged("UnselectedMultiselect");
                RaisePropertyChanged("ShowItemMultiSelector");
            }
        }

        private bool _unselectedMultiselect;
        public bool UnselectedMultiselect
        {
            get
            {
                if (_showItemMultiSelector)
                {
                    return !_itemSelected;
                }
                return false;

            }
        }

        private bool _selectedMultiselect;
        public bool SelectedMultiselect
        {
            get
            {
                if(_showItemMultiSelector)
                {
                    return _itemSelected;
                }
                return false;
            }
        }

        private bool _itemSelected;
        public bool ItemSelected
        {
            get
            {
                return _itemSelected;
            }
            set
            {
                _itemSelected = value;
                RaisePropertyChanged("ItemSelected");
                RaisePropertyChanged("UnselectedMultiselect");
                RaisePropertyChanged("SelectedMultiselect");
            }
        }



        private Uri _avatarUri;
        public Uri AvatarUri
        {
            get
            {
                return _avatarUri;
            }
            set
            {
                _avatarUri = value;
                RaisePropertyChanged("AvatarUri");
            }
        }

        private ImageSource _newMessageImageSource;
        public ImageSource NewMessageImageSource
        {
            get
            {
                return _newMessageImageSource;
            }
            set
            {
                _newMessageImageSource = value;
                RaisePropertyChanged("NewMessageImageSource");
            }
        }

        private ImageSource _emailReadImageSource;
        public ImageSource EmailReadImageSource
        {
            get
            {
                return _emailReadImageSource;
            }
            set
            {
                _emailReadImageSource = value;
                RaisePropertyChanged("EmailReadImageSource");
            }
        }

        private ImageSource _emailUnreadImageSource;
        public ImageSource EmailUnreadImageSource
        {
            get
            {
                return _emailUnreadImageSource;
            }
            set
            {
                _emailUnreadImageSource = value;
                RaisePropertyChanged("EmailUnreadImageSource");
            }
        }

        private ImageSource _multiSelectSelectedCheckboxImageSource;
        public ImageSource MultiSelectSelectedCheckboxImageSource
        {
            get
            {
                return _multiSelectSelectedCheckboxImageSource;
            }
            set
            {
                _multiSelectSelectedCheckboxImageSource = value;
                RaisePropertyChanged("MultiSelectSelectedCheckboxImageSource");
            }
        }

        private ImageSource _multiSelectUnselectedCheckboxImageSource;
        public ImageSource MultiSelectUnselectedCheckboxImageSource
        {
            get
            {
                return _multiSelectUnselectedCheckboxImageSource;
            }
            set
            {
                _multiSelectUnselectedCheckboxImageSource = value;
                RaisePropertyChanged("MultiSelectUnselectedCheckboxImageSource");
            }
        } 
 
        public RichMessageInboxListViewItemViewModel()
        {
            this.SelectCommand = new Command<string>(async (key) =>
            {
                if(key.Equals("Selected"))
                {
                    ItemSelected = false;
                }
                if (key.Equals("Unselected"))
                {
                    ItemSelected = true;
                }
            });

            NewMessageImageSource = ImageSource.FromResource("Donky.Messaging.Rich.Inbox.XamarinForms.Images.NewMessageBlank.png");

            EmailReadImageSource = ImageSource.FromResource("Donky.Messaging.Rich.Inbox.XamarinForms.Images.EmailRead.png");
            EmailUnreadImageSource = ImageSource.FromResource("Donky.Messaging.Rich.Inbox.XamarinForms.Images.EmailUnread.png");
            MultiSelectSelectedCheckboxImageSource = ImageSource.FromResource("Donky.Messaging.Rich.Inbox.XamarinForms.Images.MultiSelectSelectedCheckbox.png");
            MultiSelectUnselectedCheckboxImageSource = ImageSource.FromResource("Donky.Messaging.Rich.Inbox.XamarinForms.Images.MultiSelectUnselectedCheckbox.png");
        }


    }
}
