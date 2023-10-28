namespace YallaMasar.Data.Models.Locations;

public class LocationInfo : BaseLangEntity
{
	public string Name { get; set; }
	public string Description { get; set; }
	public string SelectionMessage { get; set; }
	public Guid LocationId { get; set; }
	public LocationEntity Location { get; set; }
}

