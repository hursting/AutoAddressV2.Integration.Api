using System.Net.Http.Headers;
using AutoAddressV2.Integration.Api.Http.Configuration;
using Polly;

namespace AutoAddressV2.Integration.Api.Http;

/// <summary>
/// <see cref="https://github.com/snatch-dev/Convey"/>
/// </summary>
public class AutoAddressHttpClient : IHttpClient
{
    
    private const string JsonContentType = "application/json";
    private readonly HttpClient _client;
    private readonly HttpClientSettings _settings;
    private readonly IHttpSerializer _serializer;

    public AutoAddressHttpClient(HttpClient httpClient, HttpClientSettings settings, IHttpSerializer serializer)
    {
        _client = httpClient;

        _settings = settings;

        _serializer = serializer;
        
        
        
    }

    public Uri? GetBaseUrl()
    {
        return _client.BaseAddress;
    }
    
    
    protected virtual async Task<HttpResult<T>> SendResultAsync<T>(string uri, HttpMethod method,
        HttpContent content = null, IHttpSerializer serializer = null)
    {
        var response = await SendAsync(uri, method, content);
        if (!response.IsSuccessStatusCode)
        {
            return new HttpResult<T>(default, response);
        }

        var stream = await response.Content.ReadAsStreamAsync();
        var result = await DeserializeJsonFromStream<T>(stream, serializer);

        return new HttpResult<T>(result, response);
    }
    
    protected virtual Task<HttpResponseMessage> SendAsync(string uri, HttpMethod method, HttpContent content = null)
        => Policy.Handle<Exception>()
            .WaitAndRetryAsync(_settings.Retries, r => TimeSpan.FromSeconds(Math.Pow(2, r)))
            .ExecuteAsync(() =>
            {
                var requestUri = uri.StartsWith("http") ? uri : $"http://{uri}";

                return GetResponseAsync(requestUri, method, content);
            });
    
    public Task<HttpResult<T>> SendResultAsync<T>(HttpRequestMessage request,
        IHttpSerializer serializer = null)
        => Policy.Handle<Exception>()
            .WaitAndRetryAsync(_settings.Retries, r => TimeSpan.FromSeconds(Math.Pow(2, r)))
            .ExecuteAsync(async () =>
            {
                var response = await _client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return new HttpResult<T>(default, response);
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var result = await DeserializeJsonFromStream<T>(stream, serializer);

                return new HttpResult<T>(result, response);
            });
    protected async Task<T> DeserializeJsonFromStream<T>(Stream stream, IHttpSerializer serializer = null)
    {
        if (stream is null || stream.CanRead is false)
        {
            return default;
        }

        serializer ??= _serializer;
        return await serializer.DeserializeAsync<T>(stream);
    }
    
    protected virtual Task<HttpResponseMessage> GetResponseAsync(string uri, HttpMethod method,
        HttpContent content = null)
        => method switch
        {
            HttpMethod.Get => _client.GetAsync(uri),
            HttpMethod.Post => _client.PostAsync(uri, content),
            HttpMethod.Put => _client.PutAsync(uri, content),
            HttpMethod.Patch => _client.PatchAsync(uri, content),
            HttpMethod.Delete => _client.DeleteAsync(uri),
            _ => throw new InvalidOperationException($"Unsupported HTTP method: {method}")
        };
    
    public void SetHeaders(IDictionary<string, string> headers)
    {
        _client.DefaultRequestHeaders.Clear();
        if (headers is null)
        {
            return;
        }

        foreach (var (key, value) in headers)
        {
            if (string.IsNullOrEmpty(key))
            {
                continue;
            }
            
            _client.DefaultRequestHeaders.TryAddWithoutValidation(key, value);
        }
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonContentType));

    }

  
    
    public Task<HttpResult<T>> GetResultAsync<T>(string uri, IHttpSerializer serializer = null)
        => SendResultAsync<T>(uri, HttpMethod.Get, serializer: serializer);
}