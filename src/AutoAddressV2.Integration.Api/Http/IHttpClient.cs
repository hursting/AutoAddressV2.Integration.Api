using System.Net.Http.Headers;

namespace AutoAddressV2.Integration.Api.Http;

/// <summary>
/// <see cref="https://github.com/snatch-dev/Convey"/>
/// </summary>
public interface IHttpClient
{
    Uri? GetBaseUrl();
  
    void SetHeaders(IDictionary<string, string> headers);
    
    Task<HttpResult<T>> GetResultAsync<T>(string uri, IHttpSerializer serializer = null);


}