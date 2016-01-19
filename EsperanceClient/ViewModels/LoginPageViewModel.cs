using Chat.Esperance.Core;
using Chat.Esperance.Core.Framework.Registration;
using Com.Xamtastic.Patterns.SmallestMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using EsperanceClient.Models;

namespace EsperanceClient.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {


        public TargetIdiom _targetIdiom = Device.Idiom;
        public TargetIdiom TargetIdiom
        {
            get
            {
                return _targetIdiom;
            }
            set
            {
                _targetIdiom = value;
                RaisePropertyChanged("TargetIdiom");
            }
        }

        public DeviceOrientation _deviceOrientation;
        public DeviceOrientation DeviceOrientation
        {
            get
            {
                return _deviceOrientation;
            }
            set
            {
                _deviceOrientation = value;
                RaisePropertyChanged("DeviceOrientation");
            }
        }


        public ICommand _loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand;
            }
            set
            {
                _loginCommand = value;
                RaisePropertyChanged("LoginCommand");
            }
        }

        public ICommand _registerCommand;
        public ICommand RegisterCommand
        {
            get
            {
                return _registerCommand;
            }
            set
            {
                _registerCommand = value;
                RaisePropertyChanged("RegisterCommand");
            }
        }

        private string _userNameText;
        public string UserNameText
        {
            get { return _userNameText; }
            set { _userNameText = value; RaisePropertyChanged("UserNameText"); }
        }

        private string _phoneNumberText;
        public string PhoneNumberText
        {
            get { return _phoneNumberText; }
            set { _phoneNumberText = value; RaisePropertyChanged("PhoneNumberText"); }
        }

        private string _passwordText;
        public string PasswordText
        {
            get { return _passwordText; }
            set { _passwordText = value; RaisePropertyChanged("PasswordText"); }
        }

        private string _confirmPasswordText;
        public string ConfirmPasswordText
        {
            get { return _confirmPasswordText; }
            set { _confirmPasswordText = value; RaisePropertyChanged("ConfirmPasswordText"); }
        }

        private string _message = "Hello Login!";
        public string Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged("Message"); }
        }

        public LoginPageViewModel()
        {
            LoginCommand = new Command<string>((key) =>
            {
                var a = 0;
            });

            RegisterCommand = new Command<string>((key) =>
            {
                IRegistrationManager registrationManager = EsperanceCore.Instance.GetService<IRegistrationManager>();

                var tokenModel = registrationManager.GetToken(
                    "http://esperanceapiserver.azurewebsites.net/token",
                    UserNameText.Trim(),
                    PasswordText,
                    "grantType",
                    "clientId",
                    "clientSecret"
                    );
            });

        }
    }
}
