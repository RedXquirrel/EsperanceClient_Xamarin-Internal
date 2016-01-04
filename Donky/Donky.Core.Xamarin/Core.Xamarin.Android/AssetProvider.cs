// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     AssetProvider class
//  Author:          Ben Moore
//  Created date:    22/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System.Threading.Tasks;
using Android.Graphics;
using Donky.Core.Assets;
using Donky.Core.Framework.Network;

namespace Donky.Core.Xamarin.Android
{
	internal class AssetProvider : IAssetProvider
	{
		private readonly IAssetHelper _assetHelper;
		private readonly IHttpClient _httpClient;

		public AssetProvider(IAssetHelper assetHelper, IHttpClient httpClient)
		{
			_assetHelper = assetHelper;
			_httpClient = httpClient;
		}

		public async Task<Bitmap> GetImageAssetAsync(string assetId)
		{
			var uri = _assetHelper.CreateUriForAsset(assetId);
			var response = await _httpClient.GetStreamAsync(uri);
			if (response.IsSuccessStatusCode)
			{
				using (response.Body)
				{
					var bitmap = await BitmapFactory.DecodeStreamAsync(response.Body);
					return bitmap;
				}
			}

			return null;
		}
	}
}