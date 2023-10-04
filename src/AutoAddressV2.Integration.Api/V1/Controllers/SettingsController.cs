using System.ComponentModel.DataAnnotations;
using AutoAddressV2.Integration.Api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Quic;
using Microsoft.Extensions.Options;

namespace AutoAddressV2.Integration.Api.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class SettingsController : ControllerBase
{
    private readonly IOptions<AppSettings> _settings;
    
    public SettingsController(IOptions<AppSettings> settings)
    {
        _settings = settings;
        
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Route("get-settings")]
    public IActionResult GetSetting()
    {
        return Ok(_settings.Value.HttpClientSettings.ApiKey);
    }
    
}