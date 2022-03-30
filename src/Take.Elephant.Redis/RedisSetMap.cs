﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Take.Elephant.Redis
{
    public class RedisSetMap<TKey, TItem> : MapBase<TKey, ISet<TItem>>, ISetMap<TKey, TItem>
    {
        private readonly ISerializer<TItem> _serializer;
        private readonly bool _useScanOnEnumeration;
        private readonly bool _supportEmptySets;
        private readonly TimeSpan _emptyIndicatorExpiration;

        public RedisSetMap(string mapName,
                           string configuration,
                           ISerializer<TItem> serializer,
                           int db = 0,
                           CommandFlags readFlags = CommandFlags.None,
                           CommandFlags writeFlags = CommandFlags.None,
                           bool useScanOnEnumeration = true,
                           bool supportEmptySets = false,
                           TimeSpan? emptyIndicatorExpiration = default)
            : this(mapName, StackExchange.Redis.ConnectionMultiplexer.Connect(configuration), serializer, db, readFlags, writeFlags, useScanOnEnumeration, supportEmptySets, emptyIndicatorExpiration)
        {
        }

        public RedisSetMap(string mapName,
                           IConnectionMultiplexer connectionMultiplexer,
                           ISerializer<TItem> serializer,
                           int db = 0,
                           CommandFlags readFlags = CommandFlags.None,
                           CommandFlags writeFlags = CommandFlags.None,
                           bool useScanOnEnumeration = true,
                           bool supportEmptySets = false,
                           TimeSpan? emptyIndicatorExpiration = default)
            : base(mapName, connectionMultiplexer, db, readFlags, writeFlags)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _useScanOnEnumeration = useScanOnEnumeration;
            _supportEmptySets = supportEmptySets;
            _emptyIndicatorExpiration = emptyIndicatorExpiration ?? TimeSpan.FromMinutes(15);
        }

        // Some methods below use tasks instead of async-await to 
        // avoid redis transaction deadlock issues.
        // See https://stackoverflow.com/questions/25976231/stackexchange-redis-transaction-methods-freezes
        // GetDatabase() may return an ITransaction (which is-a IDatabase). For an example, see the overriden impl
        // of InternalSet.GetDatabase
        // TODO: perhaps we should slap async in everything here (see "Async guidance performance optimization" in our internal wiki)
        // and leave not awaiting these methods as a responsibility for the caller
        // since they are the ones who know whether or not the provided IDatabase
        // is an instance of ITransaction

        public override async Task<bool> TryAddAsync(TKey key,
            ISet<TItem> value,
            bool overwrite = false,
            CancellationToken cancellationToken = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value is InternalSet internalSet)
            {
                return internalSet.Key.Equals(key) && overwrite;
            }

            if (!(GetDatabase() is IDatabase database))
            {
                throw new NotSupportedException("The database instance type is not supported");
            }

            var redisKey = GetRedisKey(key);
            if (await database.KeyExistsAsync(redisKey, ReadFlags).ConfigureAwait(false) && !overwrite)
            {
                return false;
            }

            var emptySetRedisKey = RedisSet<TItem>.GetEmptySetIndicatorForKey(redisKey);

            if (value == null && _supportEmptySets)
            {
                return await database.StringSetAsync(emptySetRedisKey, true.ToString(), _emptyIndicatorExpiration);
            }
            else if (value == null)
            {
                return false;
            }

            var transaction = database.CreateTransaction();
            var commandTasks = new List<Task>();
            if (overwrite)
            {
                commandTasks.Add(transaction.KeyDeleteAsync(redisKey, WriteFlags));
            }

            internalSet = CreateSet(key, transaction);

            var enumerableAsync = value.AsEnumerableAsync(cancellationToken);
            int itemsAdded = 0;
            await enumerableAsync.ForEachAsync(item =>
            {
                itemsAdded++;
                commandTasks.Add(internalSet.AddAsync(item, cancellationToken));
            }, cancellationToken).ConfigureAwait(false);

            if (_supportEmptySets)
            {
                commandTasks.Add(transaction.StringSetAsync(emptySetRedisKey, (itemsAdded > 0).ToString(), _emptyIndicatorExpiration));
            }

            var success = await transaction.ExecuteAsync(WriteFlags).ConfigureAwait(false);
            await Task.WhenAll(commandTasks).ConfigureAwait(false);

            return success;
        }

        public override async Task<ISet<TItem>> GetValueOrDefaultAsync(TKey key,
            CancellationToken cancellationToken = default)
        {
            var database = GetDatabase();
            var redisKey = GetRedisKey(key);
            var emptySetRedisKey = RedisSet<TItem>.GetEmptySetIndicatorForKey(redisKey);

            if ((_supportEmptySets
                && bool.TryParse((await database.StringGetAsync(emptySetRedisKey)).ToString(), out _))
                || await database.KeyExistsAsync(redisKey, ReadFlags).ConfigureAwait(false))
            {
                return CreateSet(key);
            }

            return null;
        }

        public virtual Task<ISet<TItem>> GetValueOrEmptyAsync(TKey key, CancellationToken cancellationToken = default)
        {
            return CreateSet(key).AsCompletedTask<ISet<TItem>>();
        }

        public override Task<bool> TryRemoveAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var database = GetDatabase();
            var redisKey = GetRedisKey(key);
            return database.KeyDeleteAsync(redisKey, WriteFlags).ContinueWith(t =>
            {
                var removed = t.Result;
                if (_supportEmptySets && removed)
                {
                    return database.KeyDeleteAsync(RedisSet<TItem>.GetEmptySetIndicatorForKey(redisKey), WriteFlags);
                }

                return Task.FromResult(removed);
            }).Unwrap();
        }

        public override Task<bool> ContainsKeyAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var database = GetDatabase();
            return database.KeyExistsAsync(GetRedisKey(key), ReadFlags);
        }

        public override Task<bool> SetAbsoluteKeyExpirationAsync(TKey key, DateTimeOffset expiration)
        {
            return base.SetAbsoluteKeyExpirationAsync(key, expiration).ContinueWith(t =>
            {
                var success = t.Result;
                if (_supportEmptySets && success && expiration - DateTimeOffset.Now < _emptyIndicatorExpiration)
                {
                    return base.SetAbsoluteKeyExpirationAsync(RedisSet<TItem>.GetEmptySetIndicatorForKey(GetRedisKey(key)), expiration);
                }

                return Task.FromResult(success);
            }).Unwrap();
        }

        public override Task<bool> SetRelativeKeyExpirationAsync(TKey key, TimeSpan ttl)
        {
            return base.SetRelativeKeyExpirationAsync(key, ttl).ContinueWith(t =>
            {
                var success = t.Result;
                if (_supportEmptySets && success && ttl < _emptyIndicatorExpiration)
                {
                    return base.SetRelativeKeyExpirationAsync(RedisSet<TItem>.GetEmptySetIndicatorForKey(GetRedisKey(key)), ttl);
                }

                return Task.FromResult(success);
            }).Unwrap();
        }

        protected virtual InternalSet CreateSet(TKey key, ITransaction transaction = null)
        {
            return new InternalSet(key,
                                   GetRedisKey(key),
                                   _serializer,
                                   ConnectionMultiplexer,
                                   Db,
                                   ReadFlags,
                                   WriteFlags,
                                   transaction,
                                   useScanOnEnumeration: _useScanOnEnumeration,
                                   supportEmptySets: _supportEmptySets,
                                   emptyIndicatorExpiration: _emptyIndicatorExpiration);
        }

        protected class InternalSet : RedisSet<TItem>
        {
            private readonly ITransaction _transaction;

            public InternalSet(TKey key,
                               string setName,
                               ISerializer<TItem> serializer,
                               IConnectionMultiplexer connectionMultiplexer,
                               int db,
                               CommandFlags readFlags,
                               CommandFlags writeFlags,
                               ITransaction transaction = null,
                               bool useScanOnEnumeration = true,
                               bool supportEmptySets = false,
                               TimeSpan? emptyIndicatorExpiration = default)
            : base(setName,
                   connectionMultiplexer,
                   serializer,
                   db,
                   readFlags,
                   writeFlags,
                   useScanOnEnumeration: useScanOnEnumeration,
                   supportEmptySets: supportEmptySets,
                   emptyIndicatorExpiration: emptyIndicatorExpiration)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                Key = key;

                _transaction = transaction;
            }

            public TKey Key { get; }

            protected override IDatabaseAsync GetDatabase()
            {
                return _transaction ?? base.GetDatabase();
            }
        }
    }
}