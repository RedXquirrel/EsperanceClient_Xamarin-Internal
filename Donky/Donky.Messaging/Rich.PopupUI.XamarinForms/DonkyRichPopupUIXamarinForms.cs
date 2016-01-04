// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DonkyRichPopupUIXamarinForms class.
//  Author:          Ben Moore
//  Created date:    31/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Threading.Tasks;
using Donky.Core;
using Donky.Core.Events;
using Donky.Core.Framework;
using Donky.Core.Framework.Extensions;
using Donky.Core.Registration;
using Donky.Core.Xamarin.Forms.Alerts;
using Donky.Messaging.Rich.Logic;
using Xamarin.Forms;

namespace Donky.Messaging.Rich.PopupUI.XamarinForms
{
	/// <summary>
	/// Entry point for the Donky Push UI for Xamarin Forms
	/// </summary>
	public static class DonkyRichPopupUIXamarinForms
	{
		public static readonly ModuleDefinition Module = new ModuleDefinition(
			"DonkyXamarinFormsRichPopupUI", AssemblyHelper.GetAssemblyVersion(typeof(DonkyRichPopupUIXamarinForms)).ToString());

		private static bool _isInitialised;
		private static readonly object Lock = new object();

		/// <summary>
		/// Initialises this module.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">DonkyXamarinFormsRichPopupUI is already initialised</exception>
		public static void Initialise()
		{
			lock (Lock)
			{
				if (_isInitialised)
				{
					throw new InvalidOperationException("DonkyXamarinFormsRichPopupUI is already initialised");
				}

				DonkyCore.Instance.RegisterModule(Module);
				DonkyCore.Instance.SubscribeToLocalEvent<RichMessageReceivedEvent>(HandleRichMessageReceived);
				DonkyCore.Instance.SubscribeToLocalEvent<AppOpenEvent>(HandleAppOpen);

				_isInitialised = true;
			}
		}

		private static void HandleAppOpen(AppOpenEvent openEvent)
		{
			DisplayUnreadMessages().ExecuteInBackground();
		}

		private static async Task DisplayUnreadMessages()
		{
			var manager = DonkyCore.Instance.GetService<IRichMessagingManager>();
			foreach (var message in await manager.GetAllUnreadAsync())
			{
				DisplayPopupForMessage(message);
			}
		}

		private static void HandleRichMessageReceived(RichMessageReceivedEvent messageEvent)
		{
			var message = messageEvent.Message;

			if (DonkyCore.Instance.GetService<IAppState>().IsOpen)
			{
				DisplayPopupForMessage(message);
			}
		}

		private static void DisplayPopupForMessage(RichMessage message)
		{
			Device.BeginInvokeOnMainThread(() => {
				var view = new RichMessageView { Message = message };
				var page = new ContentPage
				{
					Content = view
				};
				page.Disappearing += (sender, args) => view.EnsureMessageRemoved();

				DonkyCore.Instance.PublishLocalEvent(new DisplayModalPopupEvent
				{
					Page = page
				}, Module);

				DonkyCore.Instance.GetService<IRichMessagingManager>()
					.MarkMessageAsReadAsync(message.MessageId)
					.ExecuteInBackground();

				DonkyCore.Instance.PublishLocalEvent(new DecrementBadgeCountEvent(), Module);
			});
		}
	}
}