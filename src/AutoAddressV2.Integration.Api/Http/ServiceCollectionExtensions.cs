using AutoAddressV2.Integration.Api.Authentication;
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
        
        /*
         * https://stackoverflow.com/questions/66659795/addhttpclient-fails-with-defaulthttpclientfactory
         * injecting httpclient caused issues 
         */

        // 1 - authentication
        // services.AddHttpClient<IProvideAuthentication, AuthenticationService>(client =>
        // {
        //     client.BaseAddress = new Uri(httpClientOptions.BaseUrl);
        //     client.Timeout =new TimeSpan(0, 0, httpClientOptions.TimeoutInSeconds);
        // });
        
        services.AddTransient<AddDefaultHeaderHttpHandler>();
        var clientBuilder = services.AddHttpClient<IAutoAddressHttpClient, AutoAddressAutoAddressHttpClient>(opt =>
        {
            opt.BaseAddress = new Uri(httpClientOptions.BaseUrl);
            opt.Timeout =new TimeSpan(0, 0, httpClientOptions.TimeoutInSeconds);
            opt.DefaultRequestHeaders.Add("User-Agent","PostmanRuntime/7.29.2");
            opt.DefaultRequestHeaders.Add("Host","api.autoaddress.com");
        }).AddHttpMessageHandler<AddDefaultHeaderHttpHandler>();;
        
        if (httpClientBuilder != null)
        {
            httpClientBuilder.Invoke(clientBuilder);
        }

        return services;
    }
}