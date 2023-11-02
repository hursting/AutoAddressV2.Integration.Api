namespace AutoAddressV2.Integration.Api.V1.Models;

public class LookupRequest
{
    public string Signature { get; set; } = default!;
    public string Id { get; set; }= default!;
}