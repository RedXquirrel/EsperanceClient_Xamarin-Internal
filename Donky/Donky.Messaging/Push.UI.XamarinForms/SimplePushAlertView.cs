// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     SimplePushAlertView class
//  Author:          Ben Moore
//  Created date:    31/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using Donky.Core.Xamarin.Forms;
using Xamarin.Forms;

namespace Donky.Messaging.Push.UI.XamarinForms
{
	/// <summary>
	/// Basic view for rendering a Simple Push alert while the app is running. 
	/// </summary>
	public class SimplePushAlertView : Grid
	{
		private readonly Image _image;
		private readonly Grid _contentGrid;
		private readonly Label _titleLabel;
		private readonly Label _bodyLabel;

		public SimplePushAlertView()
		{
			RowDefinitions = new RowDefinitionCollection
			{
				new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
			};

			// Allow taps to pass through
			InputTransparent = true;

			if (Device.Idiom == TargetIdiom.Phone)
			{
				ColumnDefinitions = new ColumnDefinitionCollection
				{
					// Content
					new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
				};
			}
			else
			{
				ColumnDefinitions = new ColumnDefinitionCollection
				{
					// Spacer
					new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
					// Content
					new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
					// Spacer
					new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
				};
			}

			_contentGrid = new Grid
			{
				RowDefinitions = new RowDefinitionCollection
				{
					new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
					new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
					new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
				},
				ColumnDefinitions = new ColumnDefinitionCollection
				{
					new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
					new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)}
				},
				BackgroundColor = Color.Teal,
				InputTransparent = InputTransparent
			};

			// No spacers needed on phone
			_contentGrid.SetGridProperties(0, Device.Idiom == TargetIdiom.Phone ? 0 : 1);

			Children.Add(_contentGrid);

			_image = new Image
			{
				HeightRequest = 32,
				WidthRequest = 32,
				InputTransparent = InputTransparent
			};
			_contentGrid.Children.Add(_image
				.WithPadding(10)
				.SetGridProperties(0, 0, 2));

			_titleLabel = new Label
			{
				TextColor = Color.White,
				FontSize = 16,
				InputTransparent = InputTransparent
			};
			_contentGrid.Children.Add(_titleLabel
				.WithPadding(10, 10, 0, 0)
				.SetGridProperties(0, 1));

			_bodyLabel = new Label
			{
				TextColor = Color.White,
				FontSize = 12,
				WidthRequest = 320,
				InputTransparent = InputTransparent
			};
			_contentGrid.Children.Add(_bodyLabel
				.WithPadding(0, 10, 0, 0)
				.SetGridProperties(1, 1));

		}

		public void AddActionButtons(string label1, Action callback1, string label2, Action callback2)
		{
			var button1 = CreateButton(label1, callback1);
			var button2 = CreateButton(label2, callback2);

			var buttonContainer = new Grid
			{
				RowDefinitions = new RowDefinitionCollection
				{
					new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
				},
				ColumnDefinitions = new ColumnDefinitionCollection
				{
					new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
					new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
				}
			};
			buttonContainer.SetGridProperties(2, columnSpan: 2);
			ContentGrid.Children.Add(buttonContainer);

			buttonContainer.Children.Add(button1
				.WithPadding(5)
				.SetGridProperties(0, 0));

			buttonContainer.Children.Add(button2
				.WithPadding(5)
				.SetGridProperties(0, 1));
		}

		private static Button CreateButton(string label, Action callback)
		{
			var button = new Button
			{
				Text = label,
				BorderWidth = 1,
				BackgroundColor = Color.White,
				TextColor = Color.Black,
				Command = new Command(callback)
			};
			
			// HACK: button command not registering in iOS
			var gestureRecognizer = new TapGestureRecognizer();
			gestureRecognizer.Tapped += (sender, args) => callback();
			button.GestureRecognizers.Add(gestureRecognizer);
			return button;
		}

		public Image Image
		{
			get { return _image; }
		}

		public Label TitleLabel
		{
			get { return _titleLabel; }
		}

		public Label BodyLabel
		{
			get { return _bodyLabel; }
		}

		public Grid ContentGrid
		{
			get { return _contentGrid; }
		}
	}
}