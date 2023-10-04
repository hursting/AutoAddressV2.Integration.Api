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
    private readonly IHttpClient _httpClient;
    public AuthenticationService(ICacheStore cacheStore,IOptions<AppSettings> settings, IHttpClient httpClient)
    {
        _cacheStore = cacheStore;

        _settings = settings;

        _httpClient = httpClient;
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
        IDictionary<string, string> dic = new Dictionary<string, string>();
        
        dic.Add("Authorization",_settings.Value.HttpClientSettings.ApiKey);
        
        _httpClient.SetHeaders(dic);

        var ur = GetRequestUri(_settings.Value.HttpClientSettings.BaseUrl);

        var response = await _httpClient.GetResultAsync<GetTokenResponse>(ur.AbsoluteUri.ToString());

        return response.Result.Token;
    }
    
    private Uri GetRequestUri(string baseAddress)
    {
        string requestUri = $"{baseAddress}";
        var builder = new StringBuilder(requestUri);
        
        return new Uri(builder.ToString());
    }
    
    
}