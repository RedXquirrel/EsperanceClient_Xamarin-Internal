// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IAnalyticsManager interface.
//  Author:          Ben Moore
//  Created date:    21/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using Donky.Core.Events;

namespace Donky.Core.Analytics
{
	/// <summary>
	/// Analytics logic operations
	/// </summary>
	public interface IAnalyticsManager
	{
		/// <summary>
		/// Handles an App Open event.
		/// </summary>
		/// <param name="openEvent">The open event.</param>
		void HandleAppOpen(AppOpenEvent openEvent);

		/// <summary>
		/// Handles an App Close event.
		/// </summary>
		/// <param name="closeEvent">The close event.</param>
		void HandleAppClose(AppCloseEvent closeEvent);
	}
}