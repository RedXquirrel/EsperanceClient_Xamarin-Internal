// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     NotificationManager class
//  Author:          Ben Moore
//  Created date:    07/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
/*
MIT LICENCE:
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Donky.Core.Data;
using Donky.Core.Data.Entities;
using Donky.Core.Framework;
using Donky.Core.Framework.Logging;
using Donky.Core.Notifications.Content;
using Donky.Core.Registration;
using Donky.Core.Services.Notification;

namespace Donky.Core.Notifications
{
	/// <summary>
	/// Implementation of Notification logic.
	/// </summary>
	internal class NotificationManager : INotificationManager
	{
		private readonly IDonkyClientDataContext _dataContext;
		private readonly INotificationService _notificationService;
		private readonly IModuleManager _moduleManager;
		private readonly IAppState _appState;
		private readonly IJsonSerialiser _serialiser;
	    private readonly ILogger _logger;
		private readonly List<DonkyNotificationSubscription> _donkySubscriptions = new List<DonkyNotificationSubscription>();
		private readonly List<CustomNotificationSubscription> _customSubscriptions = new List<CustomNotificationSubscription>();
		private readonly List<OutboundNotificationSubscription> _outboundSubscriptions = new List<OutboundNotificationSubscription>();
		private readonly object _subscriptionLock = new object();
		private readonly SemaphoreSlim _synchroniseLock = new SemaphoreSlim(1);

		public NotificationManager(IDonkyClientDataContext dataContext, INotificationService notificationService, IModuleManager moduleManager, IAppState appState, IJsonSerialiser serialiser, ILogger logger)
		{
			_dataContext = dataContext;
			_notificationService = notificationService;
			_moduleManager = moduleManager;
			_appState = appState;
			_serialiser = serialiser;
		    _logger = logger;
		}

		public async Task<ApiResult> SynchroniseAsync()
		{
			return await ApiResult.ForOperationAsync(async () =>
			{
				// We only want to allow one sync operation at a time.
				await _synchroniseLock.WaitAsync();

                _logger.LogDebug("Starting Notification Synchronise");

				var syncNeeded = true;
				while (syncNeeded)
				{
					// Get client notifications that are pending transmission

                    var notifications = await _dataContext.ClientNotifications.GetAllAsync();
                    var clientNotifications = notifications.Select(c => c.Notification).ToList();

					await ProcessOutboundSubscriptionsAsync(clientNotifications);

					var result = await _notificationService.SynchroniseAsync(new SynchroniseRequest
					{
						ClientNotifications = clientNotifications
							.Cast<Dictionary<string, object>>().ToList(),
						IsBackground = !_appState.IsOpen
					});

					var notificationsToRetry = new List<ClientNotification>();
					foreach (var failedNotification in result.FailedClientNotifications)
					{
						var notification = new ClientNotification(failedNotification.Notification);

						switch (failedNotification.FailureReason)
						{
							case ClientNotificationFailureReason.ValidationFailure:
                                _logger.LogValidationWarning(failedNotification.ValidationFailures,
                                    "Client notification of type {0} failed validation and will not be retried.",
                                    notification.Type);
								break;

							case ClientNotificationFailureReason.Other:
								// Retry
                                _logger.LogInformation("Server reported other failure for notification of type {0}, requeuing",
                                        notification.Type);
								notificationsToRetry.Add(notification);
								break;
						}
					}
                    var temp = _dataContext.ClientNotifications;
                    var temp4 = _dataContext.ClientNotifications.CountAsync();
					// Delete processed notifications
					foreach (var toDelete in notifications)
					{
						await _dataContext.ClientNotifications.DeleteAsync(toDelete.Id);
					}
					await _dataContext.SaveChangesAsync();

				    var temp1 = _dataContext.ClientNotifications;
                    var temp3 = await _dataContext.ClientNotifications.CountAsync();

					// Handle server notifications
					foreach (var notification in result.ServerNotifications)
					{
						var serverNotification = CreateInternalServerNotification(notification);
						await HandleServerNotificationAsync(serverNotification);
					}

					// Requeue notifications that we need to retry
					await QueueClientNotificationAsync(notificationsToRetry.ToArray());

				    var temp2 = await _dataContext.ClientNotifications.CountAsync();
					// Test to see if we need to perform another sync
					syncNeeded = result.MoreNotificationsAvailable || (await _dataContext.ClientNotifications.CountAsync()) > 0;

                    _logger.LogDebug("syncNeeded: {0}", syncNeeded);
				}

                _logger.LogDebug("Exiting Notification Synchronise");

				_synchroniseLock.Release();

                }, "SynchroniseAsync");

		}


		private static ServerNotification CreateInternalServerNotification(Services.Notification.ServerNotification notification)
		{
			var serverNotification = new ServerNotification
			{
				CreatedOn = notification.CreatedOn,
				Data = notification.Data,
				NotificationId = notification.Id,
				Type = notification.Type
			};
			return serverNotification;
		}

		public void SubscribeToNotifications(ModuleDefinition module, params CustomNotificationSubscription[] subscriptions)
		{
			if (subscriptions == null || subscriptions.Length == 0)
			{
				throw new ArgumentException("At least one subscription should be specified", "subscriptions");
			}

			_moduleManager.EnsureRegistered(module);

			lock (_subscriptionLock)
			{
				_customSubscriptions.AddRange(subscriptions);
			}

            _logger.LogDebug("Exiting Notification Synchronise");
		}

		public void SubscribeToOutboundNotifications(ModuleDefinition module, params OutboundNotificationSubscription[] subscriptions)
		{
			if (subscriptions == null || subscriptions.Length == 0)
			{
				throw new ArgumentException("At least one subscription should be specified", "subscriptions");
			}

			_moduleManager.EnsureRegistered(module);

			lock (_subscriptionLock)
			{
				_outboundSubscriptions.AddRange(subscriptions);
			}

            _logger.LogDebug("Added subscription for outbound notification types {0} for module {1}",
                String.Join(",", subscriptions.Select(s => s.Type)),
                module.Name);
		}

		public async Task QueueContentNotificationsAsync(params ContentNotification[] notifications)
		{
			var clientNotifications = new List<ClientNotification>(
				notifications.Select(n => new SendContentClientNotification(n)));
            await QueueClientNotificationAsync(clientNotifications.ToArray());
		}

		public async Task<ApiResult> SendContentNotificationsAsync(params ContentNotification[] notifications)
		{
			await QueueContentNotificationsAsync(notifications);
			return await SynchroniseAsync();
		}

		public async Task<ServerNotification> GetServerNotification(string notificationId)
		{
			var notification = await _notificationService.GetNotificationAsync(notificationId);
			if (notification == null)
			{
				return null;
			}

			return CreateInternalServerNotification(notification);
		}

		public async Task QueueClientNotificationAsync(params ClientNotification[] notifications)
		{
			foreach (var notification in notifications)
			{
				var id = Guid.NewGuid();
				await _dataContext.ClientNotifications.AddOrUpdateAsync(new InternalClientNotification
				{
					Id = id,
					Notification = notification
				});

                _logger.LogInformation("Queuing client notification of type {0} with id {1}",
                    notification.Type, id);
#if DEBUG
                _logger.LogDebug("{0}", _serialiser.Serialise(notification));
#endif
			}
			await _dataContext.SaveChangesAsync();
		}

		public async Task SendClientNotificationsAsync(params ClientNotification[] notifications)
		{
			await QueueClientNotificationAsync(notifications);
			await SynchroniseAsync();
		}

		public void SubscribeToDonkyNotifications(ModuleDefinition module, params DonkyNotificationSubscription[] subscriptions)
		{
			_moduleManager.EnsureRegistered(module);

			lock (_subscriptionLock)
			{
				_donkySubscriptions.AddRange(subscriptions);
			}

            _logger.LogDebug("Added subscription for Donky notification types {0} for module {1}",
                String.Join(",", subscriptions.Select(s => s.Type)),
                module.Name);
		}

		public async Task GetAndProcessNotificationAsync(string notificationId)
		{
			var notification = await GetServerNotification(notificationId);
			if (notification != null)
			{
				await HandleServerNotificationAsync(notification);

				if((await _dataContext.ClientNotifications.CountAsync()) > 0)
				{
					await SynchroniseAsync();
				}
			}
		}

		private Task HandleServerNotificationAsync(ServerNotification notification)
		{
#if DEBUG
            _logger.LogDebug("ServerNotification of type {0}: {1}", notification.Type,
                notification.Data.ToString());
#endif
			if (notification.Type.ToLowerInvariant() == "custom")
			{
				return HandleCustomNotificationAsync(notification);
			}
			else
			{
				return HandleDonkyNotificationAsync(notification);
			}
		}

		private async Task HandleCustomNotificationAsync(ServerNotification notification)
		{
			var customType = notification.Data.Value<string>("customType");
		
            _logger.LogInformation("Processing custom notification {0} with type {1}",
                notification.NotificationId, customType);
			var subscriptions = _customSubscriptions.Where(s => s.Type == customType).ToList();
			if (subscriptions.Count == 0)
			{
				// Queue ack and report no subscriptions
				await AcknowledgeServerNotificationAsync(notification, NotificationResult.DeliveredNoSubscription, customType);
			}
			else
			{
				var acknowledged = false;

				foreach (var subscription in subscriptions)
				{
					var failed = false;
					try
					{
						await subscription.Handler(notification);

						if (!acknowledged)
						{
							await AcknowledgeServerNotificationAsync(notification, NotificationResult.Delivered, customType);
							acknowledged = true;
						}
					}
					catch (Exception exception)
					{
                        _logger.LogError(exception, "Failure in notification handler for custom type {0}", customType);
						failed = true;
					}
					if (failed)
					{
						await AcknowledgeServerNotificationAsync(notification, NotificationResult.Failed, customType);
					}
				}
			}
		}

		private async Task HandleDonkyNotificationAsync(ServerNotification notification)
		{
            _logger.LogInformation("Processing Donky notification {0} with type {1}",
                notification.NotificationId, notification.Type);

			var subscriptions = _donkySubscriptions.Where(s => s.Type == notification.Type).ToList();
			if (subscriptions.Count == 0)
			{
#if DEBUG
                _logger.LogDebug("No subscription found for notification of type {0}.  Valid types are: {1}",
                    notification.Type, String.Join(",", subscriptions.Select(s => s.Type).Distinct()));
#endif
				// Queue ack and report no subscriptions
				await AcknowledgeServerNotificationAsync(notification, NotificationResult.DeliveredNoSubscription);
			}
			else
			{
				var acknowledged = false;
				foreach (var subscription in subscriptions)
				{
					var failed = false;
					try
					{
						await subscription.Handler(notification);

						if (!acknowledged && subscriptions.Any(s => s.AutoAcknowledge))
						{
							await AcknowledgeServerNotificationAsync(notification, NotificationResult.Delivered);
							acknowledged = true;
						}
					}
					catch (Exception exception)
					{
                        _logger.LogError(exception, "Failure in notification handler for notification type {0}", notification.Type);
						failed = true;
					}
					if (failed)
					{
						await AcknowledgeServerNotificationAsync(notification, NotificationResult.Failed);
					}
				}
			}
		}

		private Task AcknowledgeServerNotificationAsync(ServerNotification notification, NotificationResult result, string customType = null)
		{
            _logger.LogInformation("Acknowledging notification {0} of type {1}{2}, result {3}",
                notification.NotificationId,
                notification.Type,
                customType == null ? String.Empty : " - " + customType,
                result);

			var acknowledgement = new ClientNotificationWithAcknowledgement
			{
				Type = "Acknowledgement",
				AcknowledgementDetail = new ClientNotificationAcknowledgement
				{
					ServerNotificationId = notification.NotificationId,
					Result = result,
					SentTime = notification.CreatedOn,
					Type = notification.Type,
					CustomNotificationType = customType
				}
			};

			return QueueClientNotificationAsync(acknowledgement);
		}

		private async Task ProcessOutboundSubscriptionsAsync(IEnumerable<ClientNotification> notifications)
		{
			foreach (var notification in notifications)
			{
				var subscriptions = _outboundSubscriptions.Where(s => s.Type == notification.Type).ToList();

				foreach (var subscription in subscriptions)
				{
					try
					{
						await subscription.Handler(notification);
					}
					catch (Exception exception)
					{
                        _logger.LogError(exception, "Failure in notification handler for outbound type {0}", notification.Type);
					}
				}
			}
		}
    }
}