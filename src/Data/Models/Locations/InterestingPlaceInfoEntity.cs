namespace YallaMasar.Data.Models.Locations
{
	public class InterestingPlaceInfoEntity : BaseLangEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public Guid InterestingPlaceId { get; set; }
		public InterestingPlaceEntity InterestingPlace { get; set; }
	}
}
