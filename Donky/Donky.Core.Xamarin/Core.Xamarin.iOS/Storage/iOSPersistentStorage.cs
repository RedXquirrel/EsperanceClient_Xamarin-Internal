// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     iOSPersistentStorage class.
//  Author:          Ben Moore
//  Created date:    18/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using Donky.Core.Framework;
using Donky.Core.Framework.Storage;
using Foundation;

namespace Donky.Core.Xamarin.iOS.Storage
{
	/// <summary>
	/// iOS implementation of persistent storage.
	/// </summary>
	internal class iOSPersistentStorage : IPersistentStorage
	{
		private const string OBJECT_FOLDER = "DonkySavedObjects";
		private const string NSUserDefaultsResourceId = "iOSPersistentStorage_NSUserDefaults";
		private const string SavedObjectResourceIdFormat = "iOSPersistentStorage_SavedObject_{0}";
		private readonly IJsonSerialiser _serialiser;

		public iOSPersistentStorage(IJsonSerialiser serialiser)
		{
			_serialiser = serialiser;
		}

		public void Set(string key, string value)
		{
			ResourceLock.Wait(NSUserDefaultsResourceId);
			try
			{
				if (value != null)
				{
					NSUserDefaults.StandardUserDefaults.SetString(value, "DonkySettings_" + key);
				}
				else
				{
					NSUserDefaults.StandardUserDefaults.RemoveObject("DonkySettings_" + key);
				}
			}
			finally
			{
				ResourceLock.Release(NSUserDefaultsResourceId);
			}
		}

		public string Get(string key)
		{
			ResourceLock.Wait(NSUserDefaultsResourceId);
			try
			{
				return NSUserDefaults.StandardUserDefaults.StringForKey("DonkySettings_" + key);
			}
			finally
			{
				ResourceLock.Release(NSUserDefaultsResourceId);
			}
		}

		public async Task SaveObjectAsync<T>(string id, T value) where T : class
		{
			using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
			{
				if (!storage.DirectoryExists(OBJECT_FOLDER))
				{
					storage.CreateDirectory(OBJECT_FOLDER);
				}

				var resourceId = String.Format(SavedObjectResourceIdFormat, id);
				await ResourceLock.WaitAsync(resourceId);
				try
				{
					using (var file = storage.OpenFile(Path.Combine(OBJECT_FOLDER, id), FileMode.Create, FileAccess.Write))
					{
						using (var writer = new StreamWriter(file))
						{
							await writer.WriteAsync(_serialiser.Serialise(value));
						}
					}
				}
				finally
				{
					ResourceLock.Release(resourceId);
				}
			}
		}

		public async Task<T> LoadObjectAsync<T>(string id) where T : class
		{
			using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
			{
				if (storage.DirectoryExists(OBJECT_FOLDER))
				{
					var path = Path.Combine(OBJECT_FOLDER, id);

					var resourceId = String.Format(SavedObjectResourceIdFormat, id);
					await ResourceLock.WaitAsync(resourceId);
					try
					{
						if (storage.FileExists(path))
						{
							using (var file = storage.OpenFile(Path.Combine(OBJECT_FOLDER, id), FileMode.Open, FileAccess.Read))
							{
								using (var reader = new StreamReader(file))
								{
									var data = await reader.ReadToEndAsync();
									return _serialiser.Deserialise<T>(data);
								}
							}
						}
					}
					finally
					{
						ResourceLock.Release(resourceId);
					}
				}

				return null;
			}
		}
	}
}