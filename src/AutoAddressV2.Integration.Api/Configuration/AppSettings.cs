using AutoAddressV2.Integration.Api.Http.Configuration;

namespace AutoAddressV2.Integration.Api.Configuration;

public sealed class AppSettings
{
    public HttpClientSettings HttpClientSettings { get; set; } = null!;
}