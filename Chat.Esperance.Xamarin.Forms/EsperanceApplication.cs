using Donky.Automation;
using Donky.Core;
using Donky.Core.Analytics;
using Donky.Core.Framework.Extensions;
using Donky.Core.Registration;
using Donky.Core.Xamarin.Forms;
using Donky.Messaging.Common;
using Donky.Messaging.Push.Logic;
using Donky.Messaging.Rich.Logic;
using Donky.Messaging.Rich.PopupUI.XamarinForms;
using System.Diagnostics;
using System.Threading.Tasks;


namespace Chat.Esperance.Xamarin.Forms
{
    public class EsperanceApplication : DonkyApplication
    {
        public EsperanceApplication()
        {
            Initialise();
        }

        private void Initialise()
        {

            DonkyCoreAnalytics.Initialise();
            DonkyAutomation.Initialise();
            DonkyCommonMessaging.Initialise();
            DonkyPushLogic.Initialise();
            DonkyRichLogic.Initialise();
            DonkyRichPopupUIXamarinForms.Initialise();
            DonkyXamarinForms.Initialise();

            DonkyCore.Instance.SubscribeToLocalEvent<SimplePushMessageReceivedEvent>(e =>
            {
                Debug.WriteLine("*** SimplePushMessageReceivedEvent");
            });

            InitInternal().ExecuteInBackground();
        }

        private static async Task InitInternal()
        {
            #region Live Donky Control, AppSpace: "Donky Quickstart" in Anthony's Xamtastic account
            var result = await DonkyCore.Instance.InitialiseAsync(
                "dYPZXSAnyNe2AS8HhSEPxOfttsrN7THfc9Ge8W4IZY8mYTqlbW6qcfZHETI95BctKWafj1Uaz3M0QefSkoGluA",
                new UserDetails
                {
                    UserId = "esperanceiosuser",
                    DisplayName = "iOS Test User"
                }
                );
            #endregion

            if (!result.Success)
            {
                // TODO: Check result for failure info
                Debug.WriteLine(string.Format("InitialiseAsync was not successful: {0}", result.OtherFailureReason));
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
        }

        protected override void OnStart()
        {
            base.OnStart();
        }
    }
}
