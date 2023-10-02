using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace AutoAddressV2.Integration.Api.Http.PipeLine;

public class AddDefaultHeaderHttpHandler : DelegatingHandler
{
    
    
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AddDefaultHeaderHttpHandler> _logger;
    private const string UserAgentHeaderKey = "User-Agent";
    private const string HostHeaderKey = "Host";
    
    public AddDefaultHeaderHttpHandler(IHttpContextAccessor httpContextAccessor, ILogger<AddDefaultHeaderHttpHandler> logger)
    {
        _logger = logger;

        _httpContextAccessor = httpContextAccessor;
    }


    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage httpResponseMessage;
        try
        {
            // string accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            // if (string.IsNullOrEmpty(accessToken))
            // {
            //     throw new Exception($"Access token is missing for the request {request.RequestUri}");
            // }
            // request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
            
            // dic.Add("Host","api.autoaddress.com");
            // dic.Add("User-Agent","PostmanRuntime/7.29.2");
            

            var headers = _httpContextAccessor.HttpContext.Request.Headers;

            if (headers.ContainsKey(HostHeaderKey) == false)
            {
                request.Headers.Add(HostHeaderKey, "api.autoaddress.com");
            }
            
            if (headers.ContainsKey(UserAgentHeaderKey) == false)
            {
                request.Headers.Add(UserAgentHeaderKey, "PostmanRuntime/7.29.2");
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
}