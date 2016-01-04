// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     DataContextBase class
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
using System.Threading.Tasks;
using Donky.Core.Framework.Logging;
using Donky.Core.Framework.Storage;

namespace Donky.Core.Framework.Data
{
	/// <summary>
	/// Base class for data contexts that use simple file-system backed lists.
	/// </summary>
	public abstract class DataContextBase : IDataContext
	{
		private readonly IPersistentStorage _storage;
	    private readonly ILogger _logger;
		private readonly Dictionary<Type, object> _entitySets = new Dictionary<Type, object>();
		private readonly object _lock = new object();

		protected DataContextBase(IPersistentStorage storage, ILogger logger)
		{
			_storage = storage;
		    _logger = logger;
		}

		/// <summary>
		/// Gets an entity set with the specified types.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <typeparam name="TId">The type of the identifier.</typeparam>
		/// <returns>A StorageBackedEntitySet</returns>
		protected ISaveableEntitySet<TEntity, TId> GetEntitySet<TEntity, TId>()
			where TEntity : class, IEntity<TId>
		{
			var key = typeof (TEntity);
			if (!_entitySets.ContainsKey(key))
			{
				lock (_lock)
				{
					if (!_entitySets.ContainsKey(key))
					{
						_entitySets[key] = new StorageBackedEntitySet<TEntity, TId>(_storage, _logger);
					}
				}
			}

			return (ISaveableEntitySet<TEntity, TId>)_entitySets[key];
		}

		/// <summary>
		/// Saves any changes to any tracked entity sets.
		/// </summary>
		/// <returns></returns>
		public async Task SaveChangesAsync()
		{
			var setsToSave = _entitySets.Select(x => x.Value).OfType<ISaveableEntitySet>();
			await Task.WhenAll(setsToSave.Select(s => s.SaveChangesAsync()));
		}
	}
}