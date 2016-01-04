// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     AndroidPersistentStorage class.
//  Author:          Ben Moore
//  Created date:    13/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Donky.Core.Framework;
using Donky.Core.Framework.Storage;

namespace Donky.Core.Xamarin.Android.Storage
{
	internal class AndroidPersistentStorage : IPersistentStorage
	{
		private const string OBJECT_FOLDER = "DonkySavedObjects";
		private const string SharedPreferencesResourceId = "AndroidPersistentStorage_SharedPreferences";
		private const string SavedObjectResourceIdFormat = "AndroidPersistentStorage_SavedObject_{0}";
		private readonly IJsonSerialiser _serialiser;

		public AndroidPersistentStorage(IJsonSerialiser serialiser)
		{
			_serialiser = serialiser;
		}

		public void Set(string key, string value)
		{
			ResourceLock.Wait(SharedPreferencesResourceId);
			try
			{
				var edit = SharedPreferences.Edit();
				edit.PutString(key, value);
				edit.Commit();
			}
			finally
			{
				ResourceLock.Release(SharedPreferencesResourceId);
			}
		}

		public string Get(string key)
		{
			ResourceLock.Wait(SharedPreferencesResourceId);
			try
			{
				return SharedPreferences.GetString(key, null);
			}
			finally
			{
				ResourceLock.Release(SharedPreferencesResourceId);
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

		private static ISharedPreferences SharedPreferences
		{
			get { return Application.Context.GetSharedPreferences("DonkySettings", FileCreationMode.Private); }
		}
	}
}