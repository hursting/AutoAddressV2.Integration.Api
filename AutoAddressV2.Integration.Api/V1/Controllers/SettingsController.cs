using System.ComponentModel.DataAnnotations;
using AutoAddressV2.Integration.Api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Quic;
using Microsoft.Extensions.Options;

namespace AutoAddressV2.Integration.Api.V1.Controllers;

public class SettingsController : ControllerBase
{
    private readonly IOptions<AppSettings> _settings;
    private readonly HttpClient _httpClient;
    private const string AuthEndpoint = "createtoken";
    
    public SettingsController(IOptions<AppSettings> settings,  HttpClient httpClient)
    {
        _settings = settings;

        _httpClient = httpClient;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Route("get-settings")]
    public IActionResult GetSetting()
    {
        
        
        
        return Ok(_settings.Value.HttpClientSettings.ApiKey);
    }
    
}