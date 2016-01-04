// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     Asset class.
//  Author:          Ben Moore
//  Created date:    06/07/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
namespace Donky.Messaging.Common
{
	/// <summary>
	/// Represents an asset / attachement.
	/// </summary>
	public class Asset
	{
		/// <summary>
		/// The Donky Asset Id
		/// </summary>
		public string AssetId { get; set; }

		/// <summary>
		/// The Mime Type
		/// </summary>
		public string MimeType { get; set; }

		/// <summary>
		/// The filename for the asset
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The size of the asset in bytes.
		/// </summary>
		public long SizeInBytes { get; set; }
	}
}