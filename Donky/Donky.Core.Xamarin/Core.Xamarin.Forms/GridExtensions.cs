// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     GridExtensions
//  Author:          Ben Moore
//  Created date:    28/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using Xamarin.Forms;

namespace Donky.Core.Xamarin.Forms
{
	public static class GridExtensions
	{
		/// <summary>
		/// Sets the grid properties on the specified view.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <param name="row">The row.</param>
		/// <param name="column">The column.</param>
		/// <param name="rowSpan">The row span.</param>
		/// <param name="columnSpan">The column span.</param>
		/// <returns></returns>
		public static View SetGridProperties(this View view, int row = 0, int column = 0, int rowSpan = 1, int columnSpan = 1)
		{
			view.SetValue(Grid.RowProperty, row);
			view.SetValue(Grid.RowSpanProperty, rowSpan);
			view.SetValue(Grid.ColumnProperty, column);
			view.SetValue(Grid.ColumnSpanProperty, columnSpan);

			return view;
		}
	}
}