// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IApnsConfigurationProvider interface.
//  Author:          Ben Moore
//  Created date:    27/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System.Threading.Tasks;
using Foundation;

namespace Donky.Core.Xamarin.iOS.Apns
{
	/// <summary>
	/// APNS configuration operations.
	/// </summary>
	public interface IApnsConfigurationProvider
	{
		/// <summary>
		/// Registers a category provider.
		/// </summary>
		/// <param name="provider">The provider.</param>
		void RegisterCategoryProvider(IApnsCategoryProvider provider);

		/// <summary>
		/// Gets all registered categories.
		/// </summary>
		/// <returns></returns>
		Task<NSSet> GetRegisteredCategoriesAsync();
	}
}