namespace AutoAddressV2.Integration.Api.Http.Configuration;

public sealed class HttpClientSettings
{
    public string BaseUrl { get; set; } = default!;
    public int TimeoutInSeconds { get; set; }
    public int Retries { get; set; }
    public string ApiKey { get; set; } = default!;
}