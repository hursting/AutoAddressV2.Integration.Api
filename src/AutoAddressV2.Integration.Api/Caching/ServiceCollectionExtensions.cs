namespace AutoAddressV2.Integration.Api.Caching;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutoAddressCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.AddSingleton<ICacheStore, MemoryCacheStore>();
        
        return services;
    }
}