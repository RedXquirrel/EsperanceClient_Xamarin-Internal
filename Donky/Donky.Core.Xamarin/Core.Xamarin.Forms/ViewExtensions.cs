// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ViewExtensions class.
//  Author:          Ben Moore
//  Created date:    31/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using Xamarin.Forms;

namespace Donky.Core.Xamarin.Forms
{
	public static class ViewExtensions
	{
		/// <summary>
		/// Adds padding to the specified view by wrapping it in a ContentView.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <param name="padding">The padding to add (all sides).</param>
		/// <returns></returns>
		public static View WithPadding(this View view, double padding)
		{
			return view.WithPadding(padding, padding, padding, padding);
		}

		/// <summary>
		/// Adds padding to the specified view by wrapping it in a ContentView.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <param name="topBottom">The top/bottom padding.</param>
		/// <param name="leftRight">The left/right padding.</param>
		/// <returns></returns>
		public static View WithPadding(this View view, double topBottom, double leftRight)
		{
			return view.WithPadding(topBottom, leftRight, topBottom, leftRight);
		}

		/// <summary>
		/// Adds padding to the specified view by wrapping it in a ContentView.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <param name="top">The top padding.</param>
		/// <param name="left">The left padding.</param>
		/// <param name="bottom">The bottom padding.</param>
		/// <param name="right">The right padding.</param>
		/// <returns></returns>
		public static View WithPadding(this View view, double top, double left, double bottom, double right)
		{
			return new ContentView
			{
				Content = view,
				Padding = new Thickness(left, top, right, bottom),
				InputTransparent = view.InputTransparent
			};
		}
	}
}