using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using YallaMasar.Data.Models;
using YallaMasar.Data.Models.Locations;
using YallaMasar.Data.Models.Providers;
using YallaMasar.Data.Models.Trips;
using YallaMasar.Data.Models.Users;

namespace YallaMasar.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
	public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options)
	{
		Transaction = null;
	}

	public DbSet<ImageEntity> Images { get; set; }
	public DbSet<LocationEntity> Locations { get; set; }
	public DbSet<LocationInfo> LocationsInfo { get; set; }
	public DbSet<TripEntity> Trips { get; set; }
	public DbSet<TripStopEntity> TripStopss { get; set; }
	public DbSet<InterestingPlaceEntity> InterestingPlaces { get; set; }
	public DbSet<InterestingPlaceInfoEntity> InterestingPlacesInfo { get; set; }
	public DbSet<ProviderEntity> Providers { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder
			.Entity<LocationEntity>()
			.HasMany(location => location.Info)
			.WithOne(info => info.Location)
			.HasForeignKey(info => info.LocationId)
			.OnDelete(DeleteBehavior.Cascade);

		builder
			.Entity<LocationEntity>()
			.HasMany(location => location.Images)
			.WithOne(image => image.Location)
			.HasForeignKey(image => image.LocationId)
			.OnDelete(DeleteBehavior.Cascade);

		builder
			.Entity<LocationEntity>()
			.HasMany(location => location.Stops)
			.WithOne(stop => stop.Location)
			.HasForeignKey(stop => stop.LocationId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.Entity<LocationEntity>()
			.HasMany(location => location.InterestingPlaces)
			.WithOne(interestingPlace => interestingPlace.Location)
			.HasForeignKey(interestingPlace => interestingPlace.LocationId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.Entity<InterestingPlaceEntity>()
			.HasMany(interestingPlace => interestingPlace.Info)
			.WithOne(info => info.InterestingPlace)
			.HasForeignKey(info => info.InterestingPlaceId)
			.OnDelete(DeleteBehavior.Cascade);

		builder
			.Entity<InterestingPlaceEntity>()
			.HasMany(interestingPlace => interestingPlace.Images)
			.WithOne(image => image.InterestingPlace)
			.HasForeignKey(image => image.InterestingPlaceId)
			.OnDelete(DeleteBehavior.Cascade);

		builder
			.Entity<TripEntity>()
			.HasMany(trip => trip.Stops)
			.WithOne(stop => stop.Trip)
			.HasForeignKey(stop => stop.TripId)
			.OnDelete(DeleteBehavior.Cascade);

		builder
			.Entity<ProviderEntity>()
			.HasOne(provider => provider.User)
			.WithOne(user => user.Provider)
			.HasForeignKey<ProviderEntity>(provider => provider.UserId)
			.OnDelete(DeleteBehavior.SetNull);
	}

	private IDbContextTransaction Transaction { get; set; }

	public void EndTransaction()
	{
		Transaction.Commit();

		Transaction = null;
	}

	public void StartTransaction() => Transaction = Database.BeginTransaction();

	public bool HasTransaction() => Transaction != null;

	public void RollBackTransaction() => Transaction.Rollback();

	public void DeleteTransaction() => Transaction = null;

}