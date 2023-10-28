using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

using YallaMasar.Data;
using YallaMasar.Data.Models.Locations;
using YallaMasar.Managers;

namespace YallaMasar.Areas.Admin.Pages.Locations
{
	public class PlaceFormModel : PageModel
	{
		public Guid Id { get; set; }
		public Guid? PlaceId { get; set; }

		private readonly AppDbContext _db;
		private readonly IStringLocalizer<SharedResource> _localizer;
		private readonly IFileManager _fileManager;

		[BindProperty]
		public InputModel Input { get; set; }

		public string CoverPath { get; set; }

		public class InputModel
		{
			public Guid LocationId { get; set; }

			[Display(Name = "Cover")]
			[DataType(DataType.Upload)]
			public IFormFile Cover { get; set; }

			[Required(ErrorMessage = "The {0} field is required.")]
			[DataType(DataType.Text)]
			[Display(Name = "NameAr")]
			public string NameAr { get; set; }

			[Required(ErrorMessage = "The {0} field is required.")]
			[DataType(DataType.Text)]
			[Display(Name = "NameEn")]
			public string NameEn { get; set; }

			[DataType(DataType.MultilineText)]
			[Display(Name = "DescriptionAr")]
			public string DescriptionAr { get; set; }

			[DataType(DataType.MultilineText)]
			[Display(Name = "DescriptionEn")]
			public string DescriptionEn { get; set; }
		}

		public PlaceFormModel(
			AppDbContext db,
			IStringLocalizer<SharedResource> localizer,
			IFileManager fileManager
		)
		{
			_db = db ?? throw new ArgumentNullException(nameof(db));
			_localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
			_fileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager));
			Input = new();
		}


		public void OnGetAddAsync(Guid id)
		{
			Id = id;
		}

		public async Task OnGetEditAsync(Guid id, Guid? placeId)
		{
			Id = id;

			await Load(placeId);
		}

		public async Task Load(Guid? id)
		{
			if (id.HasValue)
			{
				InterestingPlaceEntity location = await _db.InterestingPlaces
					.Include(company => company.Info)
					.FirstAsync(user => user.Id == id);

				PlaceId = id;
				CoverPath = location.Cover;

				var arInfo = location.Info.FirstOrDefault(x => x.Language == "ar");

				if (arInfo != null)
				{
					Input.DescriptionAr = arInfo.Description;
					Input.NameAr = arInfo.Name;
				}

				var enInfo = location.Info.FirstOrDefault(x => x.Language == "en");

				if (enInfo != null)
				{
					Input.DescriptionEn = enInfo.Description;
					Input.NameEn = enInfo.Name;
				}
			}
		}

		public async Task<IActionResult> OnPostAsync(Guid id, Guid? placeId)
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			if (Input.Cover == null || (Input.Cover.Length < 5242880 && Input.Cover.Length > 0))
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

				InterestingPlaceEntity place = new();

				if (placeId.HasValue)
				{
					place = await _db.InterestingPlaces
						 .Include(company => company.Info)
						 .FirstAsync(company => company.Id == placeId);
				}

				var arInfo = place.Info.FirstOrDefault(x => x.Language == "ar");

				arInfo ??= new()
				{
					Language = "ar",
					CreatedOn = DateTime.UtcNow
				};

				var enInfo = place.Info.FirstOrDefault(x => x.Language == "en");

				enInfo ??= new()
				{
					Language = "en",
					CreatedOn = DateTime.UtcNow
				};

				arInfo.Description = Input.DescriptionAr;
				arInfo.Name = Input.NameAr;
				arInfo.ModifiedOn = DateTime.UtcNow;
				arInfo.ModifiedBy = userId;

				enInfo.Description = Input.DescriptionEn;
				enInfo.Name = Input.NameEn;
				enInfo.ModifiedOn = DateTime.UtcNow;
				enInfo.ModifiedBy = userId;

				place.LocationId = id;

				place.Info.Add(arInfo);
				place.Info.Add(enInfo);

				if (Input.Cover != null && Input.Cover.Length > 0)
				{
					place.Cover = await _fileManager.SaveFileAsync(Folder.LocationsCover, Input.Cover);
				}

				if (placeId.HasValue)
				{
					_db.InterestingPlaces.Update(place);
				}
				else
				{
					_db.InterestingPlaces.Add(place);
				}

				await _db.SaveChangesAsync();

				return new LocalRedirectResult($"/Admin/Locations/{id}");
			}
			else
			{
				ModelState.AddModelError(string.Empty, $"{_localizer["FileTooLarge"]} - 5MB");
			}

			return Page();
		}
	}
}
