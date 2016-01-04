// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IAssetDataProvider interface
//  Author:          Ben Moore
//  Created date:    22/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System.Threading.Tasks;
using Foundation;

namespace Donky.Core.Xamarin.iOS
{
	/// <summary>
	/// Interface for accessing asset data.
	/// </summary>
	public interface IAssetDataProvider
	{
		/// <summary>
		/// Gets the data for an asset as NSData.
		/// </summary>
		/// <param name="assetId">The asset identifier.</param>
		/// <returns></returns>
		Task<NSData> GetDataForAssetAsync(string assetId);
	}
}