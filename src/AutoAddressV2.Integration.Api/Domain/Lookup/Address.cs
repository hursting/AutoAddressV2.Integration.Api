namespace AutoAddressV2.Integration.Api.Domain.Lookup;

public class Address
{
    public string id { get; set; }
    public string language { get; set; }
    public string charset { get; set; }
    public List<Line> lines { get; set; }
    public City city { get; set; }
    public Region region { get; set; }
    public Postcode postcode { get; set; }
    public Country country { get; set; }
    public List<List<string>> label { get; set; }
}