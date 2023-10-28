using YallaMasar.Data.Models.Locations;

namespace YallaMasar.Data.Models.Trips;

public class TripStopEntity : BaseEntity<int>
{
	public TimeSpan ArrivalTime { get; set; }

	public TimeSpan StayingTime { get; set; }

	public Guid LocationId { get; set; }

	public string Comment { get; set; }

	public LocationEntity Location { get; set; }

	public Guid TripId { get; set; }

	public TripEntity Trip { get; set; }
}