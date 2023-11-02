using System.ComponentModel.DataAnnotations;
using AutoAddressV2.Integration.Api.Http;
using AutoAddressV2.Integration.Api.V1.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoAddressV2.Integration.Api.V1.Controllers;


[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class SearchController : ControllerBase
{
    private readonly IAutoAddressHttpClient _httpClient;
    
    
    public SearchController(IAutoAddressHttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    [HttpGet("{address}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AutoCompleteResponse>> Get(string address, CancellationToken cancellationToken)
    {
        var uri = new Uri(_httpClient.GetBaseUrl().AbsoluteUri + Constants.EndPoints.AutoCompleteEndpoint + $"?address={address}");
        var response = await _httpClient.GetResultAsync<AutoCompleteResponse>(uri.AbsoluteUri);
        return Ok(response);
    }
    
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LookupResponse>> Get( [FromQuery] [Required] LookupRequest request, CancellationToken cancellationToken)
    {
        var uri = new Uri(_httpClient.GetBaseUrl().AbsoluteUri + Constants.EndPoints.LookupEndpoint + $"?aa3Id={request.Id}&sig={request.Signature}");
        var response = await _httpClient.GetResultAsync<LookupResponse>(uri.AbsoluteUri);
        return Ok(response);
        
    }

    
    
    
    
}