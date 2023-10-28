using Microsoft.EntityFrameworkCore;

using YallaMasar.Data;
using YallaMasar.Data.Models.Locations;
using YallaMasar.Exceptions;

namespace YallaMasar.Managers.Locations
{
	public interface ILocationsManager
	{
		Task<string> AddImage(Guid id, IFormFile fiel);
	}

	public class LocationsManager : ILocationsManager
	{
		private readonly ILogger<LocationsManager> _logger;
		private readonly AppDbContext _db;
		private readonly IFileManager _fileManager;

		public LocationsManager(
			ILogger<LocationsManager> logger,
			AppDbContext db,
			IFileManager fileManager
		)
		{
			_logger = logger;
			_db = db;
			_fileManager = fileManager;
		}

		public async Task<string> AddImage(Guid id, IFormFile file)
		{
			if (!_db.HasTransaction())
			{
				_db.StartTransaction();
			}

			string fileName = string.Empty;

			try
			{
				LocationEntity location = await _db.Locations
					.Include(location => location.Images)
					.FirstOrDefaultAsync(location => location.Id == id);

				if (location is null)
				{
					throw new ItemNotFoundException();
				}

				fileName = await _fileManager.SaveFileAsync(Folder.Locations, file);

				location.Images.Add(new()
				{
					Name = fileName,
				});

				_db.Locations.Update(location);

				await _db.SaveChangesAsync();

				_db.EndTransaction();

				return fileName;
			}
			catch (Exception ex)
			{
				if (!string.IsNullOrEmpty(fileName))
				{
					await _fileManager.DeleteFileAsync(Folder.Locations, fileName);
				}

				_logger.LogError("Error: {Message}", ex.Message);

				_db.RollBackTransaction();

				throw;
			}
		}
	}
}
