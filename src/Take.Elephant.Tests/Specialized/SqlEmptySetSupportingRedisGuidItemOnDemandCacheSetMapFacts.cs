﻿using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture;
using Shouldly;
using Take.Elephant.Memory;
using Take.Elephant.Redis;
using Take.Elephant.Specialized.Cache;
using Take.Elephant.Sql;
using Take.Elephant.Sql.Mapping;
using Take.Elephant.Tests.Redis;
using Xunit;

namespace Take.Elephant.Tests.Specialized
{
    [Collection("SqlRedis")]
    public class SqlEmptySetSupportingRedisGuidItemOnDemandCacheSetMapFacts : GuidItemOnDemandCacheSetMapFacts
    {
        private readonly SqlRedisFixture _fixture;

        public SqlEmptySetSupportingRedisGuidItemOnDemandCacheSetMapFacts(SqlRedisFixture fixture)
        {
            _fixture = fixture;
        }

        private TimeSpan EmptySetIndicatorExpiration { get; set; } = TimeSpan.FromMinutes(15);

        public string MapName => "guid-items";

        public override IMap<Guid, ISet<Item>> CreateSource()
        {
            var databaseDriver = new SqlDatabaseDriver();
            var columns = typeof(Item)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToSqlColumns();
            columns.Add("Key", new SqlType(DbType.Guid));
            var table = new Table("GuidItems", new[] { "Key", nameof(Item.GuidProperty) }, columns);
            _fixture.SqlConnectionFixture.DropTable(table.Schema, table.Name);
            var keyMapper = new ValueMapper<Guid>("Key");
            var valueMapper = new TypeMapper<Item>(table);
            return new SetMapSpyDecorator<Guid, Item>(new SqlSetMap<Guid, Item>(databaseDriver, _fixture.SqlConnectionFixture.ConnectionString, table, keyMapper, valueMapper));
        }

        public override IMap<Guid, ISet<Item>> CreateCache()
        {
            var db = 1;
            _fixture.RedisFixture.Server.FlushDatabase(db);
            var setMap = new RedisSetMap<Guid, Item>(MapName,
                                                     _fixture.RedisFixture.Connection.Configuration,
                                                     new ItemSerializer(),
                                                     db,
                                                     supportEmptySets: true,
                                                     emptyIndicatorExpiration: EmptySetIndicatorExpiration);
            return setMap;
        }

        public override OnDemandCacheMap<Guid, ISet<Item>> Create(IMap<Guid, ISet<Item>> source, IMap<Guid, ISet<Item>> cache, TimeSpan cacheExpiration = default)
        {
            return new OnDemandCacheSetMap<Guid, Item>((ISetMap<Guid, Item>)source, (ISetMap<Guid, Item>)cache, cacheExpiration, cacheEmptyValues: true );
        }

        public override ISet<Item> CreateValue(Guid key, bool populate)
        {
            var set = new Set<Item>();
            if (populate)
            {
                set.AddAsync(Fixture.Create<Item>()).Wait();
                set.AddAsync(Fixture.Create<Item>()).Wait();
                set.AddAsync(Fixture.Create<Item>()).Wait();
            }
            return set;
        }

        [Fact]
        public async Task AccessingEmptyKeyTwiceHitsTheDatabaseOnlyOnce()
        {
            // Arrange
            var cache = CreateCache();
            var source = (SetMapSpyDecorator<Guid, Item>)CreateSource();
            var map = Create(source, cache);
            var key = CreateKey();

            // Act
            await map.GetValueOrDefaultAsync(key);
            await map.GetValueOrDefaultAsync(key);

            // Assert
            var readCount = source.ReadCount;
            readCount.ShouldBe(1);
        }

        [Fact]
        public async Task AccessingEmptyKeyTwiceAfterEmptyKeyExpiresHitsTheDatabaseTwice()
        {
            // Arrange
            EmptySetIndicatorExpiration = TimeSpan.FromMilliseconds(50);
            var cache = CreateCache();
            var source = (SetMapSpyDecorator<Guid, Item>)CreateSource();
            var map = Create(source, cache);
            var key = CreateKey();

            // Act
            await map.GetValueOrDefaultAsync(key);
            await Task.Delay(EmptySetIndicatorExpiration * 2);
            await map.GetValueOrDefaultAsync(key);

            // Assert
            var readCount = source.ReadCount;
            readCount.ShouldBe(2);
        }

        [Fact]
        public async Task AddItemToEmptySetAndThenGetThroughSetMapReturnsNonEmptySet()
        {
            // Arrange
            var cache = CreateCache();
            var source = CreateSource();
            var map = Create(source, cache);
            var key = CreateKey();
            var set = await map.GetValueOrDefaultAsync(key);
            var expected = Fixture.Create<Item>();

            // Act
            await set.AddAsync(expected);
            var actual = await map.GetValueOrDefaultAsync(key);

            // Assert
            (await actual.GetLengthAsync()).ShouldBe(1);
            (await actual.AsEnumerableAsync().SingleAsync()).ShouldBe(expected);
        }

        public override async Task GetNonExistingKeyReturnsDefault()
        {
            // Arrange
            var map = Create();
            var key = CreateKey();

            // Act
            var actual = await map.GetValueOrDefaultAsync(key);

            // Assert
            AssertEquals(await actual.AsEnumerableAsync().CountAsync(), 0);
        }

        public override async Task TryRemoveExistingKeyAndValueSucceeds()
        {
            // Arrange
            var map = Create();
            var key = CreateKey();
            var value = CreateValue(key);
            await map.TryAddAsync(key, value, false);

            // Act
            var actual = await map.TryRemoveAsync(key);

            // Assert
            AssertIsTrue(actual);
            AssertEquals(await (await map.GetValueOrDefaultAsync(key)).AsEnumerableAsync().CountAsync(), 0);
        }
    }
}