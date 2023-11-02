namespace AutoAddressV2.Integration.Api.Domain.Lookup;

public class Location
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public string crs { get; set; }
    public string accuracy { get; set; }
}