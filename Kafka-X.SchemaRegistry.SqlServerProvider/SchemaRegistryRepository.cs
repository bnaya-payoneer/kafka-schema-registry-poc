using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaX;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

internal class SchemaRegistryRepository
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

    public async Task<SchemaEntity> AddAsync(SchemaEntity schema)
    {
        _context.SchemaCollection.Add(schema);
        int affected = await _context.SaveChangesAsync();
        return schema;
    }

    #endregion //  AddAsync

    #region GetAsync

    public async Task<SchemaEntity?> TryGetAsync(string key, int version)
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
}
