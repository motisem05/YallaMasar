using System.Collections.ObjectModel;

namespace YallaMasar.Data.Models.Locations
{
	public class InterestingPlaceEntity : BaseEntity
	{
		public InterestingPlaceEntity()
		{
			Images = new List<ImageEntity>();

			Info = new Collection<InterestingPlaceInfoEntity>();
		}

		public Guid LocationId { get; set; }
		public LocationEntity Location { get; set; }

		public string Cover { get; set; }

		public List<ImageEntity> Images { get; set; }
		public ICollection<InterestingPlaceInfoEntity> Info { get; set; }
	}
}
