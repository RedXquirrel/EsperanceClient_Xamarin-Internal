// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     AlertManager class.
//  Author:          Ben Moore
//  Created date:    31/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Donky.Core.Framework.Extensions;
using Donky.Core.Framework.Logging;
using Xamarin.Forms;

namespace Donky.Core.Xamarin.Forms.Alerts
{
	/// <summary>
	/// Cross platform toast alert manager.
	/// </summary>
	public class AlertManager
	{
		private bool _isMainPageFrameInitialised;
		private readonly object _lock = new object();
		private StackLayout _overlayContainer;
		private Page _mainPage;
		private readonly Queue<DisplayAlertEvent> _alertQueue = new Queue<DisplayAlertEvent>();

		private static AlertManager _instance;
		private static readonly object _instanceLock = new object();
		
		public static AlertManager Initialise()
		{
			if (_instance == null)
			{
				lock (_instanceLock)
				{
					if (_instance == null)
					{
						_instance = new AlertManager();
					}
				}
			}

			return _instance;
		}

		private AlertManager()
		{
			DonkyCore.Instance.SubscribeToLocalEvent<DisplayAlertEvent>(HandleDisplayAlert);
			DonkyCore.Instance.SubscribeToLocalEvent<DisplayModalPopupEvent>(HandleDisplayModalPopup);
			DonkyCore.Instance.SubscribeToLocalEvent<RemoveModalPopupEvent>(HandleRemoveModalPopup);
		}

		private void HandleRemoveModalPopup(RemoveModalPopupEvent theEvent)
		{
			Logger.Instance.LogDebug("Removing modal popup");
			Device.BeginInvokeOnMainThread(async () =>
			{
				EnsureFrameInitialised();
				await _mainPage.Navigation.PopModalAsync();
			});
		}

		private void HandleDisplayModalPopup(DisplayModalPopupEvent popupEvent)
		{
			Logger.Instance.LogDebug("Displaying modal popup");
			Device.BeginInvokeOnMainThread(async () =>
			{
				EnsureFrameInitialised();
				await _mainPage.Navigation.PushModalAsync(popupEvent.Page);
			});
		}

		private void HandleDisplayAlert(DisplayAlertEvent alert)
		{
			Device.BeginInvokeOnMainThread(() => DisplayAlert(alert));
		}

		private void DisplayAlert(DisplayAlertEvent alert)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				EnsureFrameInitialised();
				if (_overlayContainer != null)
				{
					lock (_lock)
					{
						_alertQueue.Enqueue(alert);
						if (_alertQueue.Count == 1)
						{
							ProcessAlertQueueAsync().ExecuteInBackground();
						}
					}
				}
			});
		}

		private async Task ProcessAlertQueueAsync()
		{
			Logger.Instance.LogDebug("Starting alert processing.");
			DisplayAlertEvent currentAlert;
			do
			{
				lock (_lock)
				{
					var count = _alertQueue.Count;
					Logger.Instance.LogDebug("Alert queue contains {0} items", count);
					currentAlert = count == 0
						? null
						: _alertQueue.Peek();
				}

				var semaphore = new SemaphoreSlim(0, 1);
				var viewEnabled = true;

				if (currentAlert != null)
				{
					var viewContainer = new StackLayout {Orientation = StackOrientation.Vertical};
					viewContainer.GestureRecognizers.Add(new TapGestureRecognizer
					{
						NumberOfTapsRequired = 1,
						Command = new Command(() =>
						{
							if (viewEnabled)
							{
								semaphore.Release();
							}
						})
					});

					currentAlert.Dismissed += (sender, args) =>
					{
						if (viewEnabled)
						{
							semaphore.Release();
						}
					};

					viewContainer.Children.Add(currentAlert.Content);

					Device.BeginInvokeOnMainThread(() => _overlayContainer.Children.Add(viewContainer));

					if (currentAlert.AutoDismiss)
					{
						var displayTime = currentAlert.DisplayTime ?? TimeSpan.FromSeconds(5);
						await semaphore.WaitAsync(displayTime);
					}
					else
					{
						await semaphore.WaitAsync();
					}

					viewEnabled = false;
					Device.BeginInvokeOnMainThread(() => _overlayContainer.Children.Remove(viewContainer));
					
					lock (_lock)
					{
						_alertQueue.Dequeue();
					}
					semaphore.Dispose();
				}
			} while (currentAlert != null);
		}

		private void EnsureFrameInitialised()
		{
			var application = Application.Current;
			if (!_isMainPageFrameInitialised || application.MainPage != _mainPage)
			{
				lock (_lock)
				{
					if (!_isMainPageFrameInitialised || application.MainPage != _mainPage)
					{
                        if (application.MainPage == null)
                        {
                            Logger.Instance.LogWarning(
                                "DonkyApplication could not initialise alert view as the main page is null.");
                            return;
                        }

                        if (application.MainPage.GetType() == typeof(NavigationPage))
					    {
                            var contentPage = application.MainPage as NavigationPage;

                            var existingContent = ((ContentPage)contentPage.CurrentPage).Content;

                            if (existingContent == null)
                            {
                                Logger.Instance.LogWarning(
                                    "DonkyApplication could not initialise alert view as the main page is not a content page.");
                                return;
                            }

                            ((ContentPage)contentPage.CurrentPage).Content = null;
                            existingContent.Parent = null;

                            var newContent = new Grid
                            {
                                RowDefinitions = new RowDefinitionCollection
							{
								new RowDefinition{ Height = new GridLength(1, GridUnitType.Auto)},
								new RowDefinition{ Height = new GridLength(1, GridUnitType.Star)}
							},
                                ColumnDefinitions = new ColumnDefinitionCollection
					    		{
								new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star)}
							},
                                IsEnabled = false
                            };

                            existingContent.SetGridProperties(0, 0, 2);
                            newContent.Children.Add(existingContent);
                            _overlayContainer = new StackLayout
                            {
                                Orientation = StackOrientation.Vertical,
                                InputTransparent = false,
                                IsEnabled = false
                            };

                            if (Device.OS == TargetPlatform.iOS)
                            {
                                // move layout under the status bar
                                _overlayContainer.Padding = new Thickness(0, 20, 0, 0);
                            }

                            newContent.Children.Add(_overlayContainer);

                            ((ContentPage)contentPage.CurrentPage).Content = newContent;

                            _mainPage = application.MainPage;
                            _isMainPageFrameInitialised = true;
					        
					    }
                        else if (application.MainPage.GetType() == typeof (ContentPage))
                        {
                            var contentPage = application.MainPage as ContentPage;

                            if (contentPage == null)
                            {
                                Logger.Instance.LogWarning(
                                    "DonkyApplication could not initialise alert view as the main page is not a content page.");
                                return;
                            }

                            var existingContent = contentPage.Content;
                            contentPage.Content = null;
                            existingContent.Parent = null;

                            var newContent = new Grid
                            {
                                RowDefinitions = new RowDefinitionCollection
							{
								new RowDefinition{ Height = new GridLength(1, GridUnitType.Auto)},
								new RowDefinition{ Height = new GridLength(1, GridUnitType.Star)}
							},
                                ColumnDefinitions = new ColumnDefinitionCollection
							{
								new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star)}
							},
                                IsEnabled = false
                            };

                            existingContent.SetGridProperties(0, 0, 2);
                            newContent.Children.Add(existingContent);
                            _overlayContainer = new StackLayout
                            {
                                Orientation = StackOrientation.Vertical,
                                InputTransparent = false,
                                IsEnabled = false
                            };

                            if (Device.OS == TargetPlatform.iOS)
                            {
                                // move layout under the status bar
                                _overlayContainer.Padding = new Thickness(0, 20, 0, 0);
                            }

                            newContent.Children.Add(_overlayContainer);

                            contentPage.Content = newContent;

                            _mainPage = application.MainPage;
                            _isMainPageFrameInitialised = true;                            
                        }
                        else
                        {
                            Logger.Instance.LogWarning(
                                "DonkyApplication could not initialise alert view as the main page is not a content page or navigation page.");
                            return;                            
                        }
					}
				}
			}
		}
	}
}