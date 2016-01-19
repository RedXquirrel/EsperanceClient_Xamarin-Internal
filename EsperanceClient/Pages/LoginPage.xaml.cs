using Com.Xamtastic.Patterns.SmallestMvvm;
using EsperanceClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EsperanceClient.Pages
{
    [ViewModelType(typeof(LoginPageViewModel))]
    public partial class LoginPage : PageBase
    {
        static bool IsPortrait(Page p) { return p.Width < p.Height; }

        public LoginPage()
        {
            InitializeComponent();

            SizeChanged += (sender, e) =>
            {
                var portrait = IsPortrait(this);

                ((LoginPageViewModel)this.BindingContext).DeviceOrientation = IsPortrait(this) ? EsperanceClient.Models.DeviceOrientation.Portrait : EsperanceClient.Models.DeviceOrientation.Landscape;
            };
        }
    }
}
