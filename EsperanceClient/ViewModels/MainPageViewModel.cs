using Chat.Esperance.Xamarin.Forms;
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
    public class MainPageViewModel : ViewModelBase
    {
        public ICommand _initialiseDonkyCommand;
        public ICommand InitialiseDonkyCommand
        {
            get
            {
                return _initialiseDonkyCommand;
            }
            set
            {
                _initialiseDonkyCommand = value;
                RaisePropertyChanged("InitialiseDonkyCommand");
            }
        }

        private string _message = "Hello SmallestMvvm!";
        public string Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged("Message"); }
        }

        public MainPageViewModel()
        {
            InitialiseDonkyCommand = new Command<string>((key) =>
            {
                EsperanceApplication.Initialise();
            });

        }
    }
}
