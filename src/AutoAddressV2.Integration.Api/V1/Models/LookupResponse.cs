using AutoAddressV2.Integration.Api.Domain;
using AutoAddressV2.Integration.Api.Domain.Lookup;

namespace AutoAddressV2.Integration.Api.V1.Models;

public class LookupResponse
{
    public string type { get; set; }
    public Message message { get; set; }
    public Address address { get; set; }
    public Data data { get; set; }
    public List<Link> links { get; set; }
}