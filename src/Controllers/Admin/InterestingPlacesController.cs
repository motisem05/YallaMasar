using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using NuGet.Protocol;

using YallaMasar.Data;
using YallaMasar.Managers;
using YallaMasar.Managers.Locations;

namespace YallaMasar.Controllers.Admin
{
	[ApiController]
	[Route("api/[controller]")]
	public class InterestingPlacesController : ControllerBase
	{
		private readonly AppDbContext _db;

		private readonly IFileManager _fileManager;
		private readonly ILocationsManager _manager;

		public InterestingPlacesController(
			AppDbContext db,
			IFileManager fileManager,
			ILocationsManager manager
		)
		{
			_db = db;
			_fileManager = fileManager;
			_manager = manager;
		}

		[HttpGet("Covers/{id:Guid}")]
		[ResponseCache(VaryByHeader = "User-Agent", Duration = 1800)]
		public async Task<IActionResult> Get([FromRoute] Guid id)
		{
			string cover = string.Empty;

			var place = await _db.InterestingPlaces.FirstOrDefaultAsync(location => location.Id == id);

			if (place != null)
			{
				cover = place.Cover;
			}

			// Didnt get anything valid
			if (string.IsNullOrEmpty(cover))
			{
				var image = await _fileManager.GetWWWRootFileAsync("img", "avatar.png");

				return File(image.Stream, image.ContentType);
			}

			var result = await _fileManager.GeFileAsync(Folder.LocationsCover, cover);

			// File not found, return default
			if (result.Stream == null || result.Stream.Length < 1)
			{
				result = await _fileManager.GetWWWRootFileAsync("img", "avatar.png");
			}

			return File(result.Stream, result.ContentType);
		}

		[HttpGet("Images/{id}")]
		[ResponseCache(VaryByHeader = "User-Agent", Duration = 1800)]
		public async Task<IActionResult> Get([FromRoute] string id)
		{
			var result = await _fileManager.GeFileAsync(Folder.Locations, id);

			// File not found, return default
			if (result.Stream == null || result.Stream.Length < 1)
			{
				result = await _fileManager.GetWWWRootFileAsync("img", "avatar.png");
			}

			return File(result.Stream, result.ContentType);
		}

		[HttpPost("Covers/{id:Guid}")]
		public async Task<IActionResult> Post([FromRoute] Guid id, [FromForm] IFormFile image)
		{
			await _manager.AddImage(id, image);

			return Ok();
		}
	}
}
