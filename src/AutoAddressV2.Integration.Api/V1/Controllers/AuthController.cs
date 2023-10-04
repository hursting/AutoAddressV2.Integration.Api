using System.Net.Http.Headers;
using System.Text;
using AutoAddressV2.Integration.Api.Configuration;
using AutoAddressV2.Integration.Api.Domain;
using AutoAddressV2.Integration.Api.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace AutoAddressV2.Integration.Api.V1.Controllers;

public class AuthController : ControllerBase
{
    private readonly IOptions<AppSettings> _settings;
    private const string createTokenEndpoint = "createtoken";
    private const string JsonContentType = "application/json";

    private readonly IAutoAddressHttpClient _autoAddressHttpClient;

    public AuthController(IOptions<AppSettings> settings, IAutoAddressHttpClient autoAddressHttpClient)
    {
        _settings = settings;

        _autoAddressHttpClient = autoAddressHttpClient;
    }
    
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Route("get-token")]
    public async Task<HttpResult<GetTokenResponse>> GetToken( CancellationToken cancellationToken)
    {
        var ur = GetRequestUri(_settings.Value.HttpClientSettings.BaseUrl);

        return  await _autoAddressHttpClient.GetResultAsync<GetTokenResponse>(ur.AbsoluteUri.ToString() + createTokenEndpoint);
        
    }
  

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Route("get-key")]
    public IActionResult GetKey()
    {
        return Ok(_settings.Value.HttpClientSettings.ApiKey);
    }
    
    
    private Uri GetRequestUri(string baseAddress, HttpMethod method, Dictionary<string,string> queryParameters)
    {
        string requestUri = $"{baseAddress}/{method.ToString()}";
        var builder = new StringBuilder(requestUri);
        
        foreach (var parameter in queryParameters)
        {
            builder.Append($"&{parameter.Key}={parameter.Value}");
        }

        return new Uri(builder.ToString());
    }
    
    private Uri GetRequestUri(string baseAddress)
    {
        string requestUri = $"{baseAddress}";
        var builder = new StringBuilder(requestUri);
        
        return new Uri(builder.ToString());
    }

}