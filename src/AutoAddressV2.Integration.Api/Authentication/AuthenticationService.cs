using System.Text;
using AutoAddressV2.Integration.Api.Caching;
using AutoAddressV2.Integration.Api.Configuration;
using AutoAddressV2.Integration.Api.Domain;
using AutoAddressV2.Integration.Api.Http;
using Microsoft.Extensions.Options;

namespace AutoAddressV2.Integration.Api.Authentication;

public class AuthenticationService : IProvideAuthentication
{
    private readonly ICacheStore _cacheStore;
    private const string AuthenticationTokenCacheKey = "AuthenticationToken";
    private readonly IOptions<AppSettings> _settings;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpSerializer _serializer; 
    
    public AuthenticationService(ICacheStore cacheStore,IOptions<AppSettings> settings, IHttpClientFactory httpClientFactory, IHttpSerializer serializer  )
    {
        _cacheStore = cacheStore;

        _settings = settings;
        
        //using httpclientfactory because of issue https://stackoverflow.com/questions/66659795/addhttpclient-fails-with-defaulthttpclientfactory
        _httpClientFactory = httpClientFactory;

        _serializer = serializer;

    }
    
    public async Task<string> GetAuthenticationToken()
    {
        var authenticatedTokenFromCache =  _cacheStore.Get<string>(AuthenticationTokenCacheKey);

        if (authenticatedTokenFromCache != null)
        {
            return (string) authenticatedTokenFromCache;
        }

        var authenticatedTokenFromAutoAddress = await GetAuthenticationTokenFromAutoAddressEndpoint();
        
        _cacheStore.Add(authenticatedTokenFromAutoAddress,AuthenticationTokenCacheKey , new TimeSpan(0,0,2,0) );

        return authenticatedTokenFromAutoAddress;
    }

    private async Task<string> GetAuthenticationTokenFromAutoAddressEndpoint()
    {
        string authorizationKey = "Authorization";
        
        using (var client = _httpClientFactory.CreateClient())
        {
            client.DefaultRequestHeaders.Add("User-Agent","PostmanRuntime/7.29.2");
            client.DefaultRequestHeaders.Add("Host","api.autoaddress.com");
            client.DefaultRequestHeaders.Add(authorizationKey, $"Basic {_settings.Value.HttpClientSettings.ApiKey}");

            var response = await client.GetAsync(_settings.Value.HttpClientSettings.BaseUrl + Constants.EndPoints.GetTokenEndpoint);

            var stream = await response.Content.ReadAsStreamAsync();
            
            var result =await _serializer.DeserializeAsync<GetTokenResponse>(stream);

            return result.Token;
        }
    }
    
    private Uri GetRequestUri(string baseAddress)
    {
        string requestUri = $"{baseAddress}";
        var builder = new StringBuilder(requestUri);
        
        return new Uri(builder.ToString());
    }
    
    
}