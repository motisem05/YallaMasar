using System;

using YallaMasar.Data.Models.Locations;

namespace YallaMasar.Data.Models;

public class ImageEntity : BaseEntity
{
	public string Name { get; set; }
	public Guid? LocationId { get; set; }
	public LocationEntity Location { get; set; }

	public Guid? InterestingPlaceId { get; set; }
	public InterestingPlaceEntity InterestingPlace { get; set; }
}
