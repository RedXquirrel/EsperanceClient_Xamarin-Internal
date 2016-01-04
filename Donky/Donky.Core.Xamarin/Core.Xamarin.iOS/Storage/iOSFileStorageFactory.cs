// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     iOSFileStorageFactory
//  Author:          Ben Moore
//  Created date:    18/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System.IO.IsolatedStorage;
using Donky.Core.Framework.Storage;

namespace Donky.Core.Xamarin.iOS.Storage
{
	public class iOSFileStorageFactory : IFileStorageFactory
	{
		public IFileStorage GetUserStore(string directory)
		{
			var storage = IsolatedStorageFile.GetUserStoreForApplication();
			if (!storage.FileExists(directory))
			{
				storage.CreateDirectory(directory);
			}

			return new iOSFileStorage(storage, directory);
		}
	}
}