// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DonkyPushUIiOS module.
//  Author:          Ben Moore
//  Created date:    21/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Donky.Core;
using Donky.Core.Configuration;
using Donky.Core.Events;
using Donky.Core.Framework;
using Donky.Core.Framework.Extensions;
using Donky.Core.Framework.Storage;
using Donky.Core.Registration;
using Donky.Core.Xamarin.iOS;
using Donky.Core.Xamarin.iOS.Apns;
using Donky.Messaging.Push.Logic;
using Donky.Messaging.Push.UI.XamarinForms;

namespace Donky.Messaging.Push.UI.iOS
{
	/// <summary>
	/// Entry point for the Donky iOS Push UI module
	/// </summary>
	public static class DonkyPushUIiOS
	{
		public static readonly ModuleDefinition Module = new ModuleDefinition(
			"DonkyXamariniOSPushUI", AssemblyHelper.GetAssemblyVersion(typeof(DonkyPushUIiOS)).ToString());

        public static string NotificationSoundFilename { get; set; }
        
        private static bool _isInitialised;
		private static readonly object Lock = new object();
		private static ApnsCategoryProvider _categoryProvider;

		/// <summary>
		/// Initialises this module.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">DonkyPushUIiOS is already initialised</exception>
		public static void Initialise()
		{
			lock (Lock)
			{
				if (_isInitialised)
				{
					throw new InvalidOperationException("DonkyPushUIiOS is already initialised");
				}

				DonkyPushUIXamarinForms.Initialise();

				DonkyCore.Instance.RegisterModule(Module);
				DonkyCore.Instance.SubscribeToLocalEvent<SimplePushMessageReceivedEvent>(HandleSimplePushReceived);
				DonkyCore.Instance.SubscribeToLocalEvent<ConfigurationUpdatedEvent>(HandleConfigurationUpdated);
				DonkyCore.Instance.SubscribeToLocalEvent<ApnsNotificationActionEvent>(HandleNotificationAction);

				_categoryProvider = new ApnsCategoryProvider(DonkyCore.Instance.GetService<IPersistentStorage>());
				DonkyCore.Instance.GetService<IApnsConfigurationProvider>().RegisterCategoryProvider(_categoryProvider);
	
				_isInitialised = true;
			}
		}

	    public static void Initialise(string notificationSoundFilename)
	    {
	        NotificationSoundFilename = notificationSoundFilename;
	        Initialise();
	    }


	    private static void HandleNotificationAction(ApnsNotificationActionEvent actionEvent)
		{
			if (actionEvent.NotificationInfo.GetValue<string>("notificationType") != "SIMPLEPUSHMSG")
			{
				return;
			}

			var option1 = actionEvent.NotificationInfo.GetValue<string>("lbl1");
			var option2 = actionEvent.NotificationInfo.GetValue<string>("lbl2");
			var action1 = actionEvent.NotificationInfo.GetValue<string>("act1");
			var action2 = actionEvent.NotificationInfo.GetValue<string>("act2");
			var userAction = actionEvent.Action == option1
				? (action1 == "Dismiss" ? "Dismissed" : "Button1")
				: (action2 == "Dismiss" ? "Dismissed" : "Button2"); 
				
			var buttonDescription = String.Format("{0}|{1}", option1, option2);
			var interactionType = actionEvent.NotificationInfo.GetValue<string>("inttype");
			var messageId = Guid.Parse(actionEvent.NotificationInfo.GetValue<string>("msgid"));

			DonkyCore.Instance.GetService<IPushMessagingManager>().HandleInteractionResultAsync(messageId,
				interactionType, buttonDescription, userAction).ExecuteInBackground();
		}

		private static void HandleConfigurationUpdated(ConfigurationUpdatedEvent configuration)
		{
			if (configuration.ConfigurationSets.ContainsKey("ButtonSets"))
			{
				HandleButtonSetConfigurationAsync(configuration).ExecuteInBackground();
			}
		}

		private static async Task HandleButtonSetConfigurationAsync(ConfigurationUpdatedEvent configuration)
		{
			var json = configuration.ConfigurationSets["ButtonSets"].GetValue("buttonSets").ToString();
			var serialiser = DonkyCore.Instance.GetService<IJsonSerialiser>();
			var buttonSets = serialiser.Deserialise<List<ButtonSetConfiguration>>(json);
			await _categoryProvider.SetConfigurationAsync(buttonSets);

			DonkyCore.Instance.PublishLocalEvent(new RefreshApnsConfigurationEvent(), Module);
		}

		private static void HandleSimplePushReceived(SimplePushMessageReceivedEvent messageEvent)
		{
			// Decrement badge count
			DonkyCore.Instance.PublishLocalEvent(new DecrementBadgeCountEvent(), Module);

			// If this was the notification we were launched from, don't display the banner
			var appState = DonkyCore.Instance.GetService<IAppState>();
			if (appState.WasOpenedFromNotification && appState.LaunchingNotificationId == messageEvent.NotificationId)
			{
				return;
			}

			// Publish event for the common UI layer
			DonkyCore.Instance.PublishLocalEvent(new DisplaySimplePushAlertEvent
			{
				MessageReceivedEvent = messageEvent
			}, Module);
		}
	}
}