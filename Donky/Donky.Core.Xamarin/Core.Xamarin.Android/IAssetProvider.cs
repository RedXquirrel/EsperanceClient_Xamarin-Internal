// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IAssetProvider interface
//  Author:          Ben Moore
//  Created date:    22/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System.Threading.Tasks;
using Android.Graphics;

namespace Donky.Core.Xamarin.Android
{
	/// <summary>
	/// Interface for accessing asset data
	/// </summary>
	public interface IAssetProvider
	{
		/// <summary>
		/// Gets a Donky image asset as a bitmap.
		/// </summary>
		/// <param name="assetId">The asset identifier.</param>
		/// <returns></returns>
		Task<Bitmap> GetImageAssetAsync(string assetId);
	}
}