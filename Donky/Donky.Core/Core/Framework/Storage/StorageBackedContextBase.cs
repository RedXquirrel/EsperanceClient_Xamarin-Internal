// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     StorageBackedContextBase class.
//  Author:          Ben Moore
//  Created date:    03/05/2015
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
using System.Threading.Tasks;
using Donky.Core.Framework.Extensions;

namespace Donky.Core.Framework.Storage
{
	internal abstract class StorageBackedContextBase
	{
		private readonly IPersistentStorage _storage;

		protected StorageBackedContextBase(IPersistentStorage storage)
		{
			_storage = storage;
		}

		protected async Task<T> GetValueAsync<T>(string storageItem) where T : class
		{
			return await _storage.LoadObjectAsync<T>(CreateId(storageItem));
		}

		protected string GetValue(string storageItem)
		{
			return _storage.Get(CreateId(storageItem));
		}

		protected bool GetBooleanValue(string storageItem)
		{
			var rawValue = GetValue(storageItem);
			if (rawValue == null)
			{
				return false;
			}

			return Boolean.Parse(rawValue);
		}

		protected DateTime? GetDateValue(string storageItem)
		{
			var rawValue = GetValue(storageItem);
			if (rawValue == null)
			{
				return null;
			}

			return DateTime.Parse(rawValue);
		}

		protected async Task SetValueAsync<T>(T value, string storageItem) where T : class
		{
		    try
		    {
                await this._storage.SaveObjectAsync(CreateId(storageItem), value);
		    }
		    catch (Exception ex)
		    {
		        

		        throw;
		    }
			
		}

		protected void SetValue(string value, string storageItem)
		{
			_storage.Set(CreateId(storageItem), value);
		}

		protected void SetValue(DateTime value, string storageItem)
		{
			SetValue(value.ToString("O"), storageItem);
		}
		protected void SetValue(bool value, string storageItem)
		{
			SetValue(value.ToString(), storageItem);
		}

		protected abstract string CreateId(string storageItem);
	}
}