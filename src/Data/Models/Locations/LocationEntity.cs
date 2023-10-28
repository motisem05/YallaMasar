using System.Collections.ObjectModel;
using YallaMasar.Data.Models.Trips;

namespace YallaMasar.Data.Models.Locations;

public class LocationEntity : BaseEntity
{
	public LocationEntity()
	{
		Info = new Collection<LocationInfo>();
		Images = new Collection<ImageEntity>();
		InterestingPlaces = new Collection<InterestingPlaceEntity>();
	}
	public string Cover { get; set; }
	public ICollection<LocationInfo> Info { get; set; }
	public ICollection<InterestingPlaceEntity> InterestingPlaces { get; set; }
	public ICollection<ImageEntity> Images { get; set; }
	public ICollection<TripStopEntity> Stops { get; set; }
}