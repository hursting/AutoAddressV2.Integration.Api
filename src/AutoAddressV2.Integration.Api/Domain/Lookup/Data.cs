namespace AutoAddressV2.Integration.Api.Domain.Lookup;

public class Data
{
    public Location location { get; set; }
    public IeAdmin ie_admin { get; set; }
    public IeBuilding ie_building { get; set; }
    public IeLocation ie_location { get; set; }
}