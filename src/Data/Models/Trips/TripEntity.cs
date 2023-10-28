using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace YallaMasar.Data.Models.Trips;

public class TripEntity : BaseEntity
{
	public TripEntity()
	{
		Stops = new Collection<TripStopEntity>();
	}

	public string Name { get; set; }
	public string Description { get; set; }
	public float Price { get; set; }
	public float SalePrice { get; set; }
	public bool RequiresVisa { get; set; }
	public bool RequiresPermit { get; set; }
	public DateTimeOffset LastRegistrationDate { get; set; }
	public DateTimeOffset LastWithdrawingDate { get; set; }
	public string ContactNumber { get; set; }
	public string ContactName { get; set; }
	public string Comments { get; set; }
	public ICollection<TripStopEntity> Stops { get; set; }
}