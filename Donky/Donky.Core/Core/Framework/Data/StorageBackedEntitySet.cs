// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     StorageBackedEntitySet class.
//  Author:          Ben Moore
//  Created date:    29/04/2015
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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Donky.Core.Framework.Logging;
using Donky.Core.Framework.Storage;

namespace Donky.Core.Framework.Data
{
	/// <summary>
	/// Implementation of a generic, local storage backed entity set.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	/// <typeparam name="TId">The type of the identifier.</typeparam>
	internal class StorageBackedEntitySet<TEntity, TId> : ISaveableEntitySet<TEntity, TId>
		where TEntity : class, IEntity<TId>
	{
		private readonly IPersistentStorage _storage;
	    private readonly ILogger _logger;
		private Dictionary<TId, TEntity> _items;
		private readonly SemaphoreSlim _lock = new SemaphoreSlim(1);
		private readonly string _storageId;

		internal StorageBackedEntitySet(IPersistentStorage storage, ILogger logger)
		{
			_storage = storage;
			_storageId = "EntitySet_" + typeof (TEntity).FullName;
		    _logger = logger;
		}

		public async Task<TEntity> GetAsync(TId id)
		{
			await EnsureInitialisedAsync();

			TEntity entity;
			_items.TryGetValue(id, out entity);

			return entity;
		}

		public async Task<List<TEntity>> GetAllAsync(Func<TEntity, bool> predicate = null)
		{
			await EnsureInitialisedAsync();

			var query = _items.Select(x => x.Value);
			if (predicate != null)
			{
				query = query.Where(predicate);
			}

			return query.ToList();
		}

		public async Task AddOrUpdateAsync(TEntity entity)
		{
			await EnsureInitialisedAsync();

			await _lock.WaitAsync();
			_items[entity.Id] = entity;
			_lock.Release();
		}

		public async Task DeleteAsync(TId id)
		{
			await EnsureInitialisedAsync();

			await _lock.WaitAsync();
			_items.Remove(id);
			_lock.Release();
		}

		public async Task<int> CountAsync()
		{
			await EnsureInitialisedAsync();

			return _items.Count;
		}

		public async Task SaveChangesAsync()
		{
			await _lock.WaitAsync();
			try
			{
				await _storage.SaveObjectAsync(_storageId, _items);
			}
			finally
			{
				_lock.Release();
			}
		}

		private async Task EnsureInitialisedAsync()
		{
			if (_items == null)
			{
				// Have to use monitor manually instead of lock due to async behaviour
				await _lock.WaitAsync();
				try
				{
					if (_items == null)
					{
						_items = await _storage.LoadObjectAsync<Dictionary<TId, TEntity>>(_storageId)
						         ?? new Dictionary<TId, TEntity>();
					}
				}
				catch (Exception exception)
				{
					_logger.LogError(exception, "Failed to load entity set {0}, re-initialising", 
						typeof(TEntity).Name);
					_items = new Dictionary<TId, TEntity>();
				}
				finally
				{
					_lock.Release();
				}
			}
		}
	}
}