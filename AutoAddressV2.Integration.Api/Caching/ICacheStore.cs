namespace AutoAddressV2.Integration.Api.Caching;

public interface ICacheStore
{
    void Remove<TItem>(string key);
    object? Get<TItem>(string key);
    void Add<TItem>(TItem item, string key, TimeSpan? expirationTime);
}