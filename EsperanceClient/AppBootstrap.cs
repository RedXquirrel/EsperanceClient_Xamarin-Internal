using Donky.Automation;
using Donky.Core;
using Donky.Core.Analytics;
using Donky.Core.Registration;
using Donky.Core.Xamarin.Forms;
using Donky.Core.Framework.Extensions;
using Donky.Messaging.Common;
using Donky.Messaging.Push.Logic;
using Donky.Messaging.Rich.Logic;
using Donky.Messaging.Rich.PopupUI.XamarinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsperanceClient
{
    public static class AppBootstrap
    {
        public static void Initialise()
        {
            // Initialise any modules (except Core) here
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
            var result = await DonkyCore.Instance.InitialiseAsync(
             "API_KEY",
             new UserDetails
             {
                 UserId = "UserIdThatYouHaveCreated",
                 DisplayName = "The Display Name of this User"
             });
            // Additional properties are also available:
            // FirstName, LastName, EmailAddress, MobileNumber, CountryCode
            // AvatarAssetId, SelectedTags, AdditionalProperties

            if (!result.Success)
            {
                // TODO: Check result for failure info
                Debug.WriteLine(string.Format("InitialiseAsync was not successful: {0}", result.OtherFailureReason));
            }
        }
    }
}
