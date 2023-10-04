using System.Net.Http.Headers;
using AutoAddressV2.Integration.Api.Authentication;
using AutoAddressV2.Integration.Api.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace AutoAddressV2.Integration.Api.Http.PipeLine;

public class AddDefaultHeaderHttpHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AddDefaultHeaderHttpHandler> _logger;
    private const string UserAgentHeaderKey = "User-Agent";
    private const string HostHeaderKey = "Host";
    private readonly IOptions<AppSettings> _settings;
    private readonly IProvideAuthentication _authenticationService;
    
    public AddDefaultHeaderHttpHandler(IProvideAuthentication authenticationService, IOptions<AppSettings> settings,IHttpContextAccessor httpContextAccessor, ILogger<AddDefaultHeaderHttpHandler> logger)
    {
        _logger = logger;

        _httpContextAccessor = httpContextAccessor;

        _settings = settings;

        _authenticationService = authenticationService;
    }


    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage httpResponseMessage;
        
        try
        {
            AddRequiredHeaders(request);

            AddApiKeyIfRequired(request, _httpContextAccessor.HttpContext.Request.Path.Value.ToUpper());

            if (request.RequestUri.AbsoluteUri.ToUpper().Contains("GET-TOKEN") == false)
            {
                string token = await _authenticationService.GetAuthenticationToken();
                
                string url = request.RequestUri.AbsoluteUri + $"&token={token}";

                request.RequestUri = new Uri(url);
            }
            
            httpResponseMessage = await base.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to run http query {RequestUri}", request.RequestUri);
            throw;
        }
        
        return httpResponseMessage;
    }

    private HttpRequestMessage AddApiKeyIfRequired(HttpRequestMessage request, string requestPath)
    {
        string authorizationKey = "Authorization";
        
        if (requestPath.Contains("GET-TOKEN"))
        {
            if (request.Headers.Any(x => x.Key.Equals(authorizationKey)) == false)
            {
                request.Headers.Add(authorizationKey, $"Basic {_settings.Value.HttpClientSettings.ApiKey}");
                    
            }
        }

        return request;
    }

    private HttpRequestMessage AddRequiredHeaders(HttpRequestMessage request)
    {
        var headers = _httpContextAccessor.HttpContext.Request.Headers;

        if (headers.ContainsKey(HostHeaderKey))
        {
            request.Headers.Remove(HostHeaderKey);
            request.Headers.Add(HostHeaderKey, "api.autoaddress.com");
        }
            
        if (headers.ContainsKey(UserAgentHeaderKey))
        {
            request.Headers.Remove(UserAgentHeaderKey);
            request.Headers.Add(UserAgentHeaderKey, "PostmanRuntime/7.29.2");
        }

        return request;
    }
}