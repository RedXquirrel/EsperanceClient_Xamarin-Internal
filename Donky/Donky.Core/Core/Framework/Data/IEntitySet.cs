// ///////////////////////////////////////////////////////////////////////////////////////////
//  Description:     IEntitySet interface.
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
using System.Threading.Tasks;

namespace Donky.Core.Framework.Data
{
	/// <summary>
	/// Defines operations for a set of entities.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	/// <typeparam name="TId">The type of the identifier.</typeparam>
	public interface IEntitySet<TEntity, TId> where TEntity : IEntity<TId>
	{
		/// <summary>
		/// Gets the entity with the specified id.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		Task<TEntity> GetAsync(TId id);

		/// <summary>
		/// Gets all entities.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <returns></returns>
		Task<List<TEntity>> GetAllAsync(Func<TEntity, bool> predicate = null);

		/// <summary>
		/// Adds or updates the specified entities.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		Task AddOrUpdateAsync(TEntity entity);

		/// <summary>
		/// Deletes the entity with the specified id.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		Task DeleteAsync(TId id);

		/// <summary>
		/// Returns the number of entities in the set.
		/// </summary>
		/// <returns></returns>
		Task<int> CountAsync();
	}
}