// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DonkyPushUIXamarinForms class.
//  Author:          Ben Moore
//  Created date:    31/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Diagnostics;
using System.Linq;
using Donky.Core;
using Donky.Core.Assets;
using Donky.Core.Framework;
using Donky.Core.Framework.Extensions;
using Donky.Core.Registration;
using Donky.Core.Xamarin.Forms.Alerts;
using Donky.Messaging.Push.Logic;
using Xamarin.Forms;

namespace Donky.Messaging.Push.UI.XamarinForms
{
	/// <summary>
	/// Entry point for the Donky Push UI for Xamarin Forms
	/// </summary>
	public static class DonkyPushUIXamarinForms
	{
		public static readonly ModuleDefinition Module = new ModuleDefinition(
			"DonkyXamarinFormsPushUI", AssemblyHelper.GetAssemblyVersion(typeof(DonkyPushUIXamarinForms)).ToString());

		private static bool _isInitialised;
		private static readonly object Lock = new object();

		/// <summary>
		/// Initialises this module.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">DonkyXamarinFormsPushUI is already initialised</exception>
		public static void Initialise()
		{
			lock (Lock)
			{
				if (_isInitialised)
				{
					throw new InvalidOperationException("DonkyXamarinFormsPushUI is already initialised");
				}

				DonkyCore.Instance.RegisterModule(Module);

				DonkyCore.Instance.SubscribeToLocalEvent<DisplaySimplePushAlertEvent>(HandleDisplaySimplePushAlert);
	
				_isInitialised = true;
			}
		}

		private static void HandleDisplaySimplePushAlert(DisplaySimplePushAlertEvent alertEvent)
		{
			var messageEvent = alertEvent.MessageReceivedEvent;

			var alertView = new SimplePushAlertView();
			var avatarId = messageEvent.Message.AvatarAssetId;
			var autoDismiss = true;
			if (!String.IsNullOrEmpty(avatarId))
			{
				var assetHelper = DonkyCore.Instance.GetService<IAssetHelper>();
				alertView.Image.Source = new UriImageSource
				{
					Uri = new Uri(assetHelper.CreateUriForAsset(avatarId))
				};
			}
            alertView.TitleLabel.Text = messageEvent.Message.SenderDisplayName;
			alertView.BodyLabel.Text = messageEvent.Message.Body;

			var displayAlertEvent = new DisplayAlertEvent
			{
				Content = alertView
			};

			if(messageEvent.PlatformButtonSet != null)
			{
				var manager = DonkyCore.Instance.GetService<IPushMessagingManager>();
				var message = messageEvent.Message;
				var buttonSet = messageEvent.PlatformButtonSet;
				var description = String.Join("|", buttonSet.ButtonSetActions.Select(a => a.Label));

				alertView.AddActionButtons(
					buttonSet.ButtonSetActions[0].Label,
					() =>
					{
						manager.HandleInteractionResultAsync(message.MessageId, buttonSet.InteractionType,
							description,
							"Button1").ExecuteInBackground();
						displayAlertEvent.Dismiss();
					},
					buttonSet.ButtonSetActions[1].Label,
					() =>
					{
						manager.HandleInteractionResultAsync(message.MessageId, buttonSet.InteractionType,
							description,
							"Button2").ExecuteInBackground();
						displayAlertEvent.Dismiss();
					});
				autoDismiss = false;
			}

			displayAlertEvent.AutoDismiss = autoDismiss;

			DonkyCore.Instance.PublishLocalEvent(displayAlertEvent, Module);
		}
	}
}