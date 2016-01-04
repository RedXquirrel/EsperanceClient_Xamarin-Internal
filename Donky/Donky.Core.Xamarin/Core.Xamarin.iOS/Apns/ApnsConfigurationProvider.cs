// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     ApnsConfigurationProvider class
//  Author:          Ben Moore
//  Created date:    27/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;

namespace Donky.Core.Xamarin.iOS.Apns
{
	internal class ApnsConfigurationProvider : IApnsConfigurationProvider
	{
		private readonly List<IApnsCategoryProvider> _providers = new List<IApnsCategoryProvider>();
		private readonly object _lock = new object();

		public void RegisterCategoryProvider(IApnsCategoryProvider provider)
		{
			lock (_lock)
			{
				if (!_providers.Contains(provider))
				{
					_providers.Add(provider);
				}
			}
		}

		public async Task<NSSet> GetRegisteredCategoriesAsync()
		{
			var categories = new NSMutableSet();
			List<IApnsCategoryProvider> providers;
			lock (_lock)
			{
				providers = _providers.ToList();
			}

			foreach (var provider in providers.ToList())
			{
				categories.AddObjects((await provider.GetCategoriesAsync()).ToArray());
			}

			return categories;
		}
	}
}