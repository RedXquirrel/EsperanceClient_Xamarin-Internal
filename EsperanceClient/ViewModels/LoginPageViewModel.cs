using Com.Xamtastic.Patterns.SmallestMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EsperanceClient.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
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

        private string _userNameText = "test text";
        public string UserNameText
        {
            get { return _userNameText; }
            set { _userNameText = value; RaisePropertyChanged("UserNameText"); }
        }

        private string _passwordText = "test text";
        public string PasswordText
        {
            get { return _passwordText; }
            set { _passwordText = value; RaisePropertyChanged("PasswordText"); }
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
                // breakpoint test
                var a = 0;
            });

        }
    }
}
