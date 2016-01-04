// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ApnsCategoryProvider class.
//  Author:          Ben Moore
//  Created date:    27/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Donky.Core.Framework.Storage;
using Donky.Core.Xamarin.iOS.Apns;
using Foundation;
using UIKit;

namespace Donky.Messaging.Push.UI.iOS
{
	/// <summary>
	/// APNS category provider for Interactive Push options.
	/// </summary>
	public class ApnsCategoryProvider : IApnsCategoryProvider
	{
		private const string STORAGE_KEY = "ApnsCategoryProvider_Configuration";
		private readonly IPersistentStorage _persistentStorage;
		private readonly string[] _suffixes = {"|F|F", "|B|F", "|B|B", "|F|B"};
		private List<ButtonSetConfiguration> _configuration;
		
		public ApnsCategoryProvider(IPersistentStorage persistentStorage)
		{
			_persistentStorage = persistentStorage;
		}

		/// <summary>
		/// Updates the configuration.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		/// <returns></returns>
		public async Task SetConfigurationAsync(List<ButtonSetConfiguration> configuration)
		{
			_configuration = configuration;
			await _persistentStorage.SaveObjectAsync(STORAGE_KEY, configuration);
		}

		/// <summary>
		/// Gets the current configuration.
		/// </summary>
		/// <returns></returns>
		public async Task<List<ButtonSetConfiguration>> GetConfigurationAsync()
		{
			return _configuration ??
			       (_configuration = await _persistentStorage.LoadObjectAsync<List<ButtonSetConfiguration>>(STORAGE_KEY));
		}

		/// <summary>
		/// Gets the categories to be registered for push notifications.
		/// </summary>
		/// <returns></returns>
		public async Task<NSSet> GetCategoriesAsync()
		{
			var set = new NSMutableSet();

			var config = await GetConfigurationAsync();
			
			// The configuration of the categories has to be on the main thread so we'll need to wait for
			// this to finish

			var waitHandle = new ManualResetEvent(false);
			
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				if (config != null)
				{
					foreach (var buttonSet in config)
					{
						foreach (var suffix in _suffixes)
						{
							var category = new UIMutableUserNotificationCategory
							{
								Identifier = buttonSet.ButtonSetId + suffix
							};

							var currentSuffix = suffix;
							var index = -1;
							var actions = buttonSet.ButtonValues.Select(v =>
								new UIMutableUserNotificationAction
								{
									ActivationMode = GetActivationMode(currentSuffix[index += 2]),
									Identifier = v,
									Title = v,
									Destructive = false,
									AuthenticationRequired = false
								})
								.Cast<UIUserNotificationAction>()
								.ToArray();
							category.SetActions(actions, UIUserNotificationActionContext.Default);
							category.SetActions(actions, UIUserNotificationActionContext.Minimal);

							set.Add(category);
						}
					}
				}

				waitHandle.Set();
			});

			// this will block the background thread, but only for a short period
			waitHandle.WaitOne();

			return set;
		}

		private UIUserNotificationActivationMode GetActivationMode(char flag)
		{
			return flag == 'F'
				? UIUserNotificationActivationMode.Foreground
				: UIUserNotificationActivationMode.Background;
		}
	}
}