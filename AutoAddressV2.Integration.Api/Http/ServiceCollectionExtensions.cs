using AutoAddressV2.Integration.Api.Http.Configuration;
using AutoAddressV2.Integration.Api.Http.PipeLine;

namespace AutoAddressV2.Integration.Api.Http;

public static class ServiceCollectionExtensions
{
    private const string SectionName = "HttpClientSettings";
    public static IServiceCollection AddAutoAddressHttpClient(this IServiceCollection services,
        IConfiguration configuration, Action<IHttpClientBuilder>? httpClientBuilder, string sectionName = SectionName)
    {
        
        HttpClientSettings? httpClientOptions = configuration.GetRequiredSection(sectionName).Get<HttpClientSettings>();
        
        services.AddSingleton(httpClientOptions);
        services.AddSingleton<IHttpSerializer, SimpleHttpSerializer>();
        var clientBuilder = services.AddHttpClient<IHttpClient, AutoAddressHttpClient>(opt =>
        {
            opt.BaseAddress = new Uri(httpClientOptions.BaseUrl);
            opt.Timeout =new TimeSpan(0, 0, httpClientOptions.TimeoutInSeconds);
        }).AddHttpMessageHandler<AddDefaultHeaderHttpHandler>();;

        if (httpClientBuilder != null)
        {
            httpClientBuilder.Invoke(clientBuilder);
        }

        return services;
    }
}