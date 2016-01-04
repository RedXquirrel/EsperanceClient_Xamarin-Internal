//// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     
//  Author:          Ben Moore
//  Created date:    07/07/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using System;
using Donky.Core;
using Donky.Core.Framework.Extensions;
using Donky.Core.Framework.Logging;
using Donky.Core.Xamarin.Forms;
using Donky.Core.Xamarin.Forms.Alerts;
using Donky.Messaging.Rich.Logic;
using Xamarin.Forms;

namespace Donky.Messaging.Rich.PopupUI.XamarinForms
{
	public class RichMessageView : Grid
	{
		public static string DismissedText = "OK";
		private readonly WebView _webView;
		private RichMessage _message;
		private bool _dismissed;

		public RichMessageView()
		{
			ColumnDefinitions = new ColumnDefinitionCollection
			{
				new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
			};
			RowDefinitions = new RowDefinitionCollection
			{
				new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
				new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
			};

			_webView = new WebView
			{
				HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true),
				VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true)
			};
			_webView.Navigating += HandleNavigation;
			_webView.SetGridProperties();
			Children.Add(_webView);

			var button = new Button {Text = DismissedText};
			button.SetGridProperties(1);
			button.Clicked += (sender, args) =>
			{
				DonkyCore.Instance.PublishLocalEvent(new RemoveModalPopupEvent(), 
					DonkyRichPopupUIXamarinForms.Module);
				EnsureMessageRemoved();
			};
			Children.Add(button);
		}

		public void EnsureMessageRemoved()
		{
			if (!_dismissed)
			{
				_dismissed = true;
				DonkyCore.Instance.GetService<IRichMessagingManager>()
					.DeleteMessagesAsync(Message.MessageId)
					.ExecuteInBackground();
			}
		}

		public RichMessage Message
		{
			get { return _message; }
			set
			{
				_message = value;
				var html = value.ExpiryTimeStamp.HasValue && value.ExpiryTimeStamp.Value <= DateTime.UtcNow
					? value.ExpiredBody
					: value.Body;
				_webView.Source = new HtmlWebViewSource
				{
					Html = html
				};
				Logger.Instance.LogDebug("Set webview source to {0}", html);
			}
		}

		private void HandleNavigation(object sender, WebNavigatingEventArgs e)
		{
			// Need to open in the browser on the device, not in the webview
			if (e.Url.StartsWith("http"))
			{
				try
				{
					var uri = new Uri(e.Url);
					Device.OpenUri(uri);
				}
				catch (Exception exception)
				{
					Logger.Instance.LogWarning(exception.Message);
				}

				e.Cancel = true;
			}
		}
	}	
}