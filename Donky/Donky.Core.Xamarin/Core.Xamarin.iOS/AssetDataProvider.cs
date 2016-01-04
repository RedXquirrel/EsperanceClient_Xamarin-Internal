// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     AssetDataProvider class.
//  Author:          Ben Moore
//  Created date:    22/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Threading.Tasks;
using Donky.Core.Assets;
using Donky.Core.Framework.Logging;
using Foundation;

namespace Donky.Core.Xamarin.iOS
{
	internal class AssetDataProvider : IAssetDataProvider
	{
		private readonly IAssetHelper _assetHelper;

		public AssetDataProvider(IAssetHelper assetHelper)
		{
			_assetHelper = assetHelper;
		}

		public async Task<NSData> GetDataForAssetAsync(string assetId)
		{
			NSData data = null;
			await Task.Run(() =>
			{
				try
				{
					var url = _assetHelper.CreateUriForAsset(assetId);
					Logger.Instance.LogDebug("Attempting to download asset from {0}", url);
					var uri = new Uri(url);
					var nsUrl = new NSUrl(uri.GetComponents (UriComponents.HttpRequestUrl, UriFormat.UriEscaped));
					data = NSData.FromUrl(nsUrl);
				}
				catch (Exception exception)
				{
					Logger.Instance.LogError(exception, "Failed to download asset {0}", assetId);
				}
			});

			return data;
		}
	}
}