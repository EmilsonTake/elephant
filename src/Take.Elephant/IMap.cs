﻿using System.Threading;
using System.Threading.Tasks;

namespace Take.Elephant
{
    /// <summary>
    /// Defines a mapper that provides fast access to a value using a key.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IMap<TKey, TValue>
    {
        /// <summary>
        /// Tries to add an item.
        /// </summary>
        /// <param name="key">The item key</param>
        /// <param name="value">The value.</param>
        /// <param name="overwrite">Indicates if the item should be overwritten if the key already exists.</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see langword="true"/> if the item was added; <see langword="false"/> otherwise.</returns>
        Task<bool> TryAddAsync(TKey key, TValue value, bool overwrite = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the value for the key if the exists or default for the type, if not.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The mapper value if the key exists; otherwise, the <see cref="TValue"/> default value.</returns>
        Task<TValue> GetValueOrDefaultAsync(TKey key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tries the remove the item for the key if it exists on the map.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> TryRemoveAsync(TKey key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if the key exists on the map.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ContainsKeyAsync(TKey key, CancellationToken cancellationToken = default);
    }
}
