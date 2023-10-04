using AutoAddressV2.Integration.Api.Domain.AutoComplete;

namespace AutoAddressV2.Integration.Api.V1.Models;

public class AutoCompleteResponse
{
    public string type { get; set; }
    public Message message { get; set; }
    public List<Option> options { get; set; }
    public List<Link> links { get; set; }
}