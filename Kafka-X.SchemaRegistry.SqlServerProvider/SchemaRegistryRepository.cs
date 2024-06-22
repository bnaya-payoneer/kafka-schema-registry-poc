using Avro;
using Avro.Specific;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
//using Avro;
//using Avro.Specific;
//using Avro.Util;
//using Avro.Reflect;

namespace KafkaX;

internal class SchemaRegistryRepository : ISchemaStorageProvider
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

    #region TryGetAsync

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

    #endregion //  TryGetAsync

    #region GetSchemaAsync

    async Task<Schema> ISchemaStorageProvider.GetSchemaAsync<T>(int version)
    {
        string key = typeof(T).FullName ?? throw new ArgumentNullException(nameof(T));
        SchemaEntity? entity = await TryGetAsync(key, version);
        if (entity == null)
        {
            throw new SchemaNotFoundException(key, version);
        }
        var result = Schema.Parse(entity.Definition);
        return result;
    }

    #endregion //  GetSchemaAsync

    #region GetOrAddSchemaAsync

    async Task<Schema> ISchemaStorageProvider.GetOrAddSchemaAsync<T>(int version)
    {
        string key = typeof(T).FullName ?? throw new ArgumentNullException(nameof(T));
        SchemaEntity? entity = await TryGetAsync(key, version);
        if (entity == null)
        {
            string definition = typeof(T).GetAvroSchema();
            entity = new SchemaEntity { Key = key, Version = version, Definition = definition };
            entity = await AddAsync(entity);
        }
        var result = Schema.Parse(entity.Definition);
        return result;
    }

    #endregion //  GetOrAddSchemaAsync
}
