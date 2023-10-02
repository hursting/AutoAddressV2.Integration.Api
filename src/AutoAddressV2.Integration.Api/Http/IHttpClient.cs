using System.Net.Http.Headers;

namespace AutoAddressV2.Integration.Api.Http;

public interface IHttpClient
{
  
    void SetHeaders(IDictionary<string, string> headers);
    
    Task<HttpResult<T>> GetResultAsync<T>(string uri, IHttpSerializer serializer = null);


}