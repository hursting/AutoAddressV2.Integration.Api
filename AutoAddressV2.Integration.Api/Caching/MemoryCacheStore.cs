using Microsoft.Extensions.Caching.Memory;

namespace AutoAddressV2.Integration.Api.Caching;

public class MemoryCacheStore : ICacheStore
{
    private readonly IMemoryCache _memoryCache;
    private readonly TimeSpan _defaultTenMinuteTimeSpan;

    public MemoryCacheStore(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        
        _defaultTenMinuteTimeSpan = new TimeSpan(0,10,0);
    }
    
    public void Add<TItem>(TItem item, string key, TimeSpan? expirationTime = null)
    {
       
        TimeSpan timespan = expirationTime.HasValue ? expirationTime.Value : _defaultTenMinuteTimeSpan;

        _memoryCache.Set(key, item, timespan);
    }

    

    public object? Get<TItem>(string key)
    {
        if (this._memoryCache.TryGetValue(key, out TItem? value))
        {
            return value;
        }

        return null;
    }

    public void Remove<TItem>(string key)
    {
        _memoryCache.Remove(key);
    }
}