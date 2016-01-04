// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     PushActionIntentService class.
//  Author:          Ben Moore
//  Created date:    24/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Donky.Core;
using Donky.Core.Framework;
using Donky.Core.Framework.Extensions;
using Donky.Core.Framework.Logging;
using Donky.Messaging.Push.Logic;
using Uri = Android.Net.Uri;

namespace Donky.Messaging.Push.UI.Android
{
	/// <summary>
	/// Android intent service for handling interactive push
	/// </summary>
	[Service]
	public class PushActionIntentService : IntentService
	{
		public const string CancelAction = "donky.messaging.pushAction.cancel";
		public const string OpenAction = "donky.messaging.pushAction.openApplication";
		public const string DeepLinkAction = "donky.messaging.pushAction.openDeepLink";
		public const string ButtonActionData = "DonkyDeepLink";
		public const string NativeNotificationId = "DonkyNativeNotificationId";
		public const string UserAction = "DonkyUserAction";
		public const string ButtonSet = "DonkyButtonSet";
		public const string Message = "DonkyMessage";

		protected override void OnHandleIntent(Intent intent)
		{
			HandleIntentAsync(intent).ExecuteInBackground();
		}

		private async Task HandleIntentAsync(Intent intent)
		{
			var action = intent.Action;
			Logger.Instance.LogDebug("PushActionIntent - {0}", action);
			await QueueResult(intent);
			CancelNotification(intent);

			Intent newIntent = null;
			switch (action)
			{
				case CancelAction:
					// No further action required
					return;

				case OpenAction:
					// Launch the app
					newIntent = GetLaunchIntent(intent);
					break;

				case DeepLinkAction:
					newIntent = GetDeepLinkIntent(intent);
					break;

				default:
					Logger.Instance.LogError("Unexpected action of {0} passed to PushActionIntentService",
						action);
					break;
			}

			if (newIntent != null)
			{
				StartActivity(newIntent);
			}
		}

		private Intent GetDeepLinkIntent(Intent current)
		{
			var intent = new Intent();
			intent.PutExtras(current.Extras);

			var deepLink = current.Extras.GetString(ButtonActionData);
			intent.SetData(Uri.Parse(deepLink));

			intent.SetAction(Intent.ActionView);
			intent.AddCategory(Intent.CategoryDefault);
			intent.AddFlags(ActivityFlags.NewTask);

			// Make sure there is a valid target
			if (!PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly).Any())
			{
				Logger.Instance.LogWarning("Could not find an intent that matched the link {0}", deepLink);
				intent = GetLaunchIntent(current);
			}

			return intent;
		}

		private Intent GetLaunchIntent(Intent current)
		{
			if (DonkyCore.Instance.GetService<IAppState>().IsOpen)
			{
				return null;
			}

			var packageName = Application.Context.PackageName;
			var intent = PackageManager.GetLaunchIntentForPackage(packageName);

			intent.PutExtras(current.Extras);

			intent.SetAction(Intent.ActionView);
			intent.AddCategory(Intent.CategoryDefault);
			intent.AddFlags(ActivityFlags.NewTask);

			return intent;
		}

		private async Task QueueResult(Intent intent)
		{
			var serialiser = DonkyCore.Instance.GetService<IJsonSerialiser>();
			var manager = DonkyCore.Instance.GetService<IPushMessagingManager>();

			var message = serialiser.Deserialise<SimplePushMessage>(intent.Extras.GetString(Message));
			var userAction = intent.Extras.GetString(UserAction);
			var buttonSet = serialiser.Deserialise<ButtonSet>(intent.Extras.GetString(ButtonSet));

			await manager.HandleInteractionResultAsync(message.MessageId, buttonSet.InteractionType,
				String.Join("|", buttonSet.ButtonSetActions.Select(a => a.Label)),
				userAction);
		}

		private void CancelNotification(Intent intent)
		{
			var nativeNotificationId = intent.Extras.GetInt(NativeNotificationId);
			NotificationManager.FromContext(Application.Context).Cancel(nativeNotificationId);
		}
	}
}