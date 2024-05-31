using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Cache
{
    public class MemoryCache
    {
        private readonly IMemoryCache _cache;

        public MemoryCache(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public void SetCache(string key, object value)
        {
            _cache.Set(key, value, TimeSpan.FromMinutes(5)); // Cache for 5 minutes
        }

        public T GetCache<T>(string key)
        {
            if (_cache.TryGetValue<T>(key, out T cacheEntry))
            {
                return cacheEntry;
            }
            return default(T); // or handle cache miss as needed
        }

        public void RemoveCache(string key)
        {
            _cache.Remove(key);
        }
    }
}
