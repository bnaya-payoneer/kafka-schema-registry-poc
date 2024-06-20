using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avro;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
//using Avro;
//using Avro.Specific;
//using Avro.Util;
//using Avro.Reflect;

namespace KafkaX;

internal class SchemaRegistryRepository: ISchemaStorageProvider
{
    private readonly SchemaRegistryContext _context;
    private readonly IMemoryCache _cache;
    private static readonly MemoryCacheEntryOptions CACHE_GET_OPTIONS = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

    #region Ctor

    public SchemaRegistryRepository(
        SchemaRegistryContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache; 
    }

    #endregion //  Ctor

    #region AddAsync

    private async Task<SchemaEntity> AddAsync(SchemaEntity schema)
    {
        schema = schema with { ModifiedDate = DateTimeOffset.UtcNow };
        _context.SchemaCollection.Add(schema);
        int affected = await _context.SaveChangesAsync();
        return schema;
    }

    #endregion //  AddAsync

    #region GetAsync

    private async Task<SchemaEntity?> TryGetAsync(string key, int version)
    {
        var cacheKey = $"SchemaRegistry:{key}:{version}";
        if (!_cache.TryGetValue(cacheKey, out SchemaEntity? entity))
        {
            entity = await _context.SchemaCollection.FindAsync(key, version);
            if (entity != null)
            {
                _cache.Set(cacheKey, entity, CACHE_GET_OPTIONS);
            }
        }
        return entity;
    }

    #endregion //  GetAsync

    async Task<Schema> ISchemaStorageProvider.GetSchemaAsync(string key, int version)
    {
        SchemaEntity? schema = await TryGetAsync(key, version);
        if(schema == null)
        {
            throw new SchemaNotFoundException(key, version);
        }
        var id = new SchemaIdentifier(schema.Key, schema.Version);
        var date = schema.ModifiedDate ?? throw new ArgumentNullException(nameof(schema.ModifiedDate));
        return new Schema(id, schema.Definition, date);
    }

    async Task<Schema> ISchemaStorageProvider.GetOrAddSchemaAsync(string key, ProvideNewSchema callback, int version)
    {
        SchemaEntity? schema = await TryGetAsync(key, version);
        if(schema == null)
        {
            byte[] definition = await callback(key, version);
            schema = new SchemaEntity { Key = key, Version = version, Definition = definition };
            schema = await AddAsync(schema);
        }
        var id = new SchemaIdentifier(schema.Key, schema.Version);
        var date = schema.ModifiedDate ?? throw new ArgumentNullException(nameof(schema.ModifiedDate));
        return new Schema(id, schema.Definition, date);
    }

    async Task<Schema> ISchemaStorageProvider.GetOrAddSchemaAsync<T>(int version)
    {
        string key = typeof(T).FullName ?? throw new ArgumentNullException(nameof(T));
        SchemaEntity? schema = await TryGetAsync(key, version);
        if(schema == null)
        {
            string schemaData = AvroSchemaGenerator.GenerateSchema<T>();
            byte[] definition = Encoding.UTF8.GetBytes(schemaData);
            schema = new SchemaEntity { Key = key, Version = version, Definition = definition };
            schema = await AddAsync(schema);
        }
        var id = new SchemaIdentifier(schema.Key, schema.Version);
        var date = schema.ModifiedDate ?? throw new ArgumentNullException(nameof(schema.ModifiedDate));
        return new Schema(id, schema.Definition, date);
    }
}
