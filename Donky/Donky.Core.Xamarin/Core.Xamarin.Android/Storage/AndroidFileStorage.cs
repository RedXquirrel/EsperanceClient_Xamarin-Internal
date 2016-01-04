// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     AndroidFileStorage class.
//  Author:          Ben Moore
//  Created date:    13/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
using System.IO;
using System.IO.IsolatedStorage;
using Donky.Core.Framework.Storage;

namespace Donky.Core.Xamarin.Android.Storage
{
	/// <summary>
	/// Android implementatin of file storage
	/// </summary>
	public class AndroidFileStorage : IFileStorage
	{
		private readonly IsolatedStorageFile _storage;
		private readonly string _directory;

		public AndroidFileStorage(IsolatedStorageFile storage, string directory)
		{
			_storage = storage;
			_directory = directory;
		}

		public void Dispose()
		{
			if (_storage != null)
			{
				_storage.Dispose();
			}
		}

		public bool FileExists(string fileName)
		{
			var path = Path.Combine(_directory, fileName);
			return _storage.FileExists(path);
		}

		public void RenameFile(string fileName, string newName)
		{
			var sourcePath = Path.Combine(_directory, fileName);
			var targetPath = Path.Combine(_directory, newName);
			_storage.MoveFile(sourcePath, targetPath);
		}

		public void DeleteFileIfExists(string fileName)
		{
			var path = Path.Combine(_directory, fileName);
			if (_storage.FileExists(path))
			{
				_storage.DeleteFile(path);
			}
		}

		public long GetFileSize(string fileName)
		{
			var path = Path.Combine(_directory, fileName);
			using (var file = _storage.OpenFile(path, FileMode.Open, FileAccess.Read))
			{
				return file.Length;
			}
		}

		public StreamReader OpenReader(string fileName)
		{
			var path = Path.Combine(_directory, fileName);

			return new StreamReader(_storage.OpenFile(path, FileMode.Open, FileAccess.Read));
		}

		public StreamWriter OpenWriter(string fileName, bool append)
		{
			var path = Path.Combine(_directory, fileName);
			Stream stream;
			if (_storage.FileExists(path))
			{
				stream = _storage.OpenFile(path, append ? FileMode.Append : FileMode.Open, FileAccess.Write);
			}
			else
			{
				stream = _storage.OpenFile(path, FileMode.Create, FileAccess.Write);
			}

			return new StreamWriter(stream);
		}
	}
}