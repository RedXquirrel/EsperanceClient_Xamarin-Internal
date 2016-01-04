// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     RichMessagingManager class.
//  Author:          Ben Moore
//  Created date:    06/07/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Donky.Core.Framework;
using Donky.Core.Framework.Events;
using Donky.Core.Framework.Extensions;
using Donky.Core.Notifications;
using Donky.Messaging.Common;
using Donky.Messaging.Rich.Logic.Data;
using Donky.Core.Configuration;

namespace Donky.Messaging.Rich.Logic
{
	internal class RichMessagingManager : IRichMessagingManager
	{
        private readonly IConfigurationManager _configurationManager;
		private readonly IRichDataContext _context;
		private readonly ICommonMessagingManager _commonMessagingManager;
		private readonly IJsonSerialiser _serialiser;
		private readonly IEventBus _eventBus;

        public RichMessagingManager(IRichDataContext context, ICommonMessagingManager commonMessagingManager, IJsonSerialiser serialiser, IEventBus eventBus, IConfigurationManager configurationManager)
		{
			_context = context;
			_commonMessagingManager = commonMessagingManager;
			_serialiser = serialiser;
			_eventBus = eventBus;
            _configurationManager = configurationManager;

            DeleteRichMessageAvailabilityDaysExpirations();
		}

		public async Task<IEnumerable<RichMessage>> GetAllAsync()
		{
			var data = await _context.RichMessages.GetAllAsync();
			return data.Select(d => d.Message);
		}

		public async Task<IEnumerable<RichMessage>> GetAllUnreadAsync()
		{
			var data = await _context.RichMessages.GetAllAsync(
				d => !d.Message.ReadTimestamp.HasValue);
			return data.Select(d => d.Message);
		}

		public async Task<RichMessage> GetByIdAsync(Guid messageId)
		{
			var data = await _context.RichMessages.GetAsync(messageId);
			return data == null
				? null
				: data.Message;
		}

		public async Task MarkMessageAsReadAsync(Guid messageId)
		{
			var data = await _context.RichMessages.GetAsync(messageId);
			if (data != null)
			{
				data.Message.ReadTimestamp = DateTime.UtcNow;
				await _commonMessagingManager.NotifyMessageReadAsync(data.Message);
				await _context.RichMessages.AddOrUpdateAsync(data);
				await _context.SaveChangesAsync();
			}
		}

		public async Task DeleteMessagesAsync(params Guid[] messageIds)
		{
			foreach (var id in messageIds)
			{
				await _context.RichMessages.DeleteAsync(id);
			}

			await _context.SaveChangesAsync();
		}

		public async Task HandleRichMessageAsync(ServerNotification notification)
		{
			var message = _serialiser.Deserialise<RichMessage>(notification.Data.ToString());
			message.ReceivedTimestamp = DateTime.UtcNow;
			var expired = message.ExpiryTimeStamp.HasValue && message.ExpiryTimeStamp <= DateTime.UtcNow;

			if (!expired)
			{
				var data = new RichMessageData
				{
					Id = message.MessageId,
					Message = message
				};

				await _context.RichMessages.AddOrUpdateAsync(data);
				await _context.SaveChangesAsync();

				_eventBus.PublishAsync(new RichMessageReceivedEvent
				{
					Message = message,
					NotificationId = notification.NotificationId,
					Publisher = DonkyRichLogic.Module
				}).ExecuteInBackground();
			}

			await _commonMessagingManager.NotifyMessageReceivedAsync(message, notification);
		}

        private async Task DeleteRichMessageAvailabilityDaysExpirations()
        {
            var richMessageAvailabilityDays = _configurationManager.GetValue<string>("RichMessageAvailabilityDays");
            var timeNow = DateTime.UtcNow;

            var messages = await GetAllAsync();

            List<Guid> deletionQueue = new List<Guid>();

            foreach(var message in messages)
            {
                var richMessageReceivedTime = message.ReceivedTimestamp;
                var richMessageAvailabilityExpiry = ((DateTime)richMessageReceivedTime).AddDays(Convert.ToInt32(richMessageAvailabilityDays));

                if(richMessageAvailabilityExpiry <= timeNow)
                {
                    deletionQueue.Add(message.MessageId);
                }
            }

            if(deletionQueue.Count > 0)
            {
                await DeleteMessagesAsync(deletionQueue.ToArray());
            }
        }
	}
}