// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IApnsCategoryProvider interface.
//  Author:          Ben Moore
//  Created date:    27/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System.Threading.Tasks;
using Foundation;

namespace Donky.Core.Xamarin.iOS.Apns
{
	/// <summary>
	/// Defines an APNS category provider
	/// </summary>
	public interface IApnsCategoryProvider
	{
		/// <summary>
		/// Gets the categories to be registered for push notifications.
		/// </summary>
		/// <returns></returns>
		Task<NSSet> GetCategoriesAsync();
	}
}