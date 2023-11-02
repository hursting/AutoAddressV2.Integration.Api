namespace AutoAddressV2.Integration.Api.Domain.Lookup;

public class IeBuilding
{
    public int ecadId { get; set; }
    public int buildingTypeId { get; set; }
    public bool underConstruction { get; set; }
    public string buildingUse { get; set; }
    public bool vacant { get; set; }
}