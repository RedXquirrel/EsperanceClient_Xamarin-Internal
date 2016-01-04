// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IFileStorage interface.
//  Author:          Ben Moore
//  Created date:    13/05/2015
//  Copyright:       Donky Networks Ltd 2015
// ///////////////////////////////////////////////////////////////////////////////////////////
/*
MIT LICENCE:
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE. */
using System;
using System.IO;

namespace Donky.Core.Framework.Storage
{
	/// <summary>
	/// Interface for accessing file storage.
	/// </summary>
	public interface IFileStorage : IDisposable
	{
		/// <summary>
		/// Tests for existance of a file.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns></returns>
		bool FileExists(string fileName);

		/// <summary>
		/// Renames a file.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="newName">The new name.</param>
		void RenameFile(string fileName, string newName);

		/// <summary>
		/// Deletes a file if it exists.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		void DeleteFileIfExists(string fileName);

		/// <summary>
		/// Gets the size of a file.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns></returns>
		long GetFileSize(string fileName);

		/// <summary>
		/// Opens a reader for the specified file.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns></returns>
		StreamReader OpenReader(string fileName);

		/// <summary>
		/// Opens a writer for the specified file.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="append">If true, an existing file will be appended to, otherwise a new file will be created.</param>
		/// <returns></returns>
		StreamWriter OpenWriter(string fileName, bool append);
	}
}