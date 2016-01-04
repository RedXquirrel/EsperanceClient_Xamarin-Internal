// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     PushMessagingManager class.
//  Author:          Ben Moore
//  Created date:    14/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Linq;
using System.Threading.Tasks;
using Donky.Core;
using Donky.Core.Events;
using Donky.Core.Framework;
using Donky.Core.Framework.Extensions;
using Donky.Core.Framework.Logging;
using Donky.Core.Notifications;
using Donky.Messaging.Common;
using Donky.Messaging.Push.Logic.Data;

namespace Donky.Messaging.Push.Logic
{
	internal class PushMessagingManager : IPushMessagingManager
	{
		private const string INTERACTIVE_PLATFORM = "Mobile";
		private readonly IJsonSerialiser _serialiser;
		private readonly ICommonMessagingManager _commonMessagingManager;
		private readonly IDonkyCore _donkyCore;
		private readonly INotificationManager _notificationManager;
		private readonly IEnvironmentInformation _environmentInformation;
		private readonly IPushDataContext _pushDataContext;

		public PushMessagingManager(IJsonSerialiser serialiser, ICommonMessagingManager commonMessagingManager, IDonkyCore donkyCore, INotificationManager notificationManager, IEnvironmentInformation environmentInformation, IPushDataContext pushDataContext)
		{
			_serialiser = serialiser;
			_commonMessagingManager = commonMessagingManager;
			_donkyCore = donkyCore;
			_notificationManager = notificationManager;
			_environmentInformation = environmentInformation;
			_pushDataContext = pushDataContext;

			_donkyCore.SubscribeToLocalEvent<AppOpenEvent>(HandleAppOpen);
		}

		public async Task HandleSimplePushAsync(ServerNotification notification)
		{
			var message = _serialiser.Deserialise<SimplePushMessage>(notification.Data.ToString());
			var expired = message.ExpiryTimeStamp.HasValue && message.ExpiryTimeStamp <= DateTime.UtcNow;
			await _commonMessagingManager.NotifyMessageReceivedAsync(message, notification);

			if (!expired)
			{
				var messageEvent = new SimplePushMessageReceivedEvent
				{
					NotificationId = notification.NotificationId,
					Message = message
				};

				// See if we have interactive config for this platform.
				if (message.ButtonSets != null)
				{
					messageEvent.PlatformButtonSet = message.ButtonSets
						.SingleOrDefault(b => b.Platform == INTERACTIVE_PLATFORM);

					await ProcessPendingActionsOrSaveMessage(message);
				}

				_donkyCore.PublishLocalEvent(messageEvent, DonkyPushLogic.Module);
			}
		}

		public async Task HandleInteractionResultAsync(Guid messageId, string interactionType, string buttonDescription, string userAction)
		{
			var interactionTime = DateTime.UtcNow;
			var message = await _pushDataContext.SimplePushMessages.GetAsync(messageId);
			if (message != null)
			{
				// We have the message, process the result
				await SendInteractionResultAsync(message.Message, interactionType, buttonDescription, userAction, interactionTime);
			}
			else
			{
				Logger.Instance.LogInformation("Queuing interaction result {0} for message {1}", userAction, messageId);
				// Queue the message for later
				await _pushDataContext.MessageInteractions.AddOrUpdateAsync(new PendingMessageInteraction
				{
					Id = messageId,
					InteractionType = interactionType,
					ButtonDescription = buttonDescription,
					UserAction = userAction,
					InteractionTime = interactionTime
				});

				await _pushDataContext.SaveChangesAsync();
			}
		}

		private async Task SendInteractionResultAsync(Message message, string interactionType, string buttonDescription, string userAction, DateTime interactionTime)
		{
			var notification = CreateInteractionResultNotification(message, interactionType, buttonDescription, userAction, interactionTime);
			await _notificationManager.QueueClientNotificationAsync(notification);

			var storedMessage = await _pushDataContext.SimplePushMessages.GetAsync(message.MessageId);
			if (storedMessage != null)
			{
				// We are done with this message now, delete it
				await _pushDataContext.SimplePushMessages.DeleteAsync(storedMessage.Id);
				await _pushDataContext.SaveChangesAsync();
			}
		}

		private async Task ProcessPendingActionsOrSaveMessage(SimplePushMessage message)
		{
			// Do we have any pending actions for this message?
			var pendingAction = await _pushDataContext.MessageInteractions.GetAsync(message.MessageId);
			if (pendingAction != null)
			{
				Logger.Instance.LogInformation("Found pending action of {0} for message {1}", 
					pendingAction.UserAction, message.MessageId);
				await _pushDataContext.MessageInteractions.DeleteAsync(pendingAction.Id);

				await SendInteractionResultAsync(message, pendingAction.InteractionType,
					pendingAction.ButtonDescription, pendingAction.UserAction, pendingAction.InteractionTime);
			}
			else
			{
				// Store the message for later
				await _pushDataContext.SimplePushMessages.AddOrUpdateAsync(new PendingSimplePushMessage
				{
					Id = message.MessageId,
					Message = message
				});
			}

			await _pushDataContext.SaveChangesAsync();
		}
		
		private ClientNotification CreateInteractionResultNotification(Message message, string interactionType, string buttonDescription, string userAction, DateTime interactionTime)
		{
			var notification = new ClientNotification
			{
				{"type", "InteractionResult"},
				{"senderInternalUserId", message.SenderInternalUserId},
				{"messageId", message.MessageId},
				{"senderMessageId", message.SenderMessageId},
				{"timeToInteractionSeconds", (int)(interactionTime - message.SentTimestamp).TotalSeconds},
				{"interactionTimeStamp", interactionTime},
				{"interactionType", interactionType},
				{"buttonDescription", buttonDescription},
				{"userAction", userAction},
				{"operatingSystem", _environmentInformation.OperatingSystem},
				{"messageSentTimestamp", message.SentTimestamp},
				{"contextItems", message.ContextItems},
			};

			return notification;
		}

		private void HandleAppOpen(AppOpenEvent appOpen)
		{
			CleanupExpiredDataAsync().ExecuteInBackground();
		}

		private async Task CleanupExpiredDataAsync()
		{
			var threshold = DateTime.UtcNow.AddDays(-60);
			var messages = (await _pushDataContext.SimplePushMessages.GetAllAsync(
				x => x.Message.SentTimestamp <= threshold)).ToList();
			var interactions = (await _pushDataContext.MessageInteractions.GetAllAsync(
				x => x.InteractionTime <= threshold)).ToList();

			foreach (var message in messages)
			{
				await _pushDataContext.SimplePushMessages.DeleteAsync(message.Id);
			}

			foreach (var interaction in interactions)
			{
				await _pushDataContext.MessageInteractions.DeleteAsync(interaction.Id);
			}

			await _pushDataContext.SaveChangesAsync();
		}
	}
}