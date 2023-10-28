using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

using YallaMasar.Data;
using YallaMasar.Data.Models.Locations;
using YallaMasar.Managers;

namespace YallaMasar.Areas.Admin.Pages.Locations;

public class FormModel : PageModel
{
	public Guid? Id { get; set; }

	private readonly AppDbContext _db;
	private readonly IStringLocalizer<SharedResource> _localizer;
	private readonly IFileManager _fileManager;

	[BindProperty]
	public InputModel Input { get; set; }

	public string CoverPath { get; set; }

	public class InputModel
	{
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

		[DataType(DataType.MultilineText)]
		[Display(Name = "SelectionMessageAr")]
		public string SelectionMessageAr { get; set; }

		[DataType(DataType.MultilineText)]
		[Display(Name = "SelectionMessageEn")]
		public string SelectionMessageEn { get; set; }
	}

	public FormModel(
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


	public void OnGetAddAsync()
	{
	}

	public async Task OnGetEditAsync(Guid? id)
	{
		await Load(id);
	}

	public async Task Load(Guid? id)
	{
		if (id.HasValue)
		{
			LocationEntity location = await _db.Locations
				.Include(company => company.Info)
				.FirstAsync(user => user.Id == id);

			Id = id;
			CoverPath = location.Cover;

			var arInfo = location.Info.FirstOrDefault(x => x.Language == "ar");

			if (arInfo != null)
			{
				Input.DescriptionAr = arInfo.Description;
				Input.SelectionMessageAr = arInfo.SelectionMessage;
				Input.NameAr = arInfo.Name;
			}

			var enInfo = location.Info.FirstOrDefault(x => x.Language == "en");

			if (enInfo != null)
			{
				Input.DescriptionEn = enInfo.Description;
				Input.SelectionMessageEn = enInfo.SelectionMessage;
				Input.NameEn = enInfo.Name;
			}
		}
	}

	public async Task<IActionResult> OnPostAsync(Guid? id)
	{
		if (ModelState.IsValid)
		{
			if (Input.Cover == null || (Input.Cover.Length < 5242880 && Input.Cover.Length > 0))
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

				LocationEntity location = new();

				if (id.HasValue)
				{
					location = await _db.Locations
						 .Include(company => company.Info)
						 .FirstAsync(company => company.Id == id);
				}

				var arInfo = location.Info.FirstOrDefault(x => x.Language == "ar");

				if (arInfo is null)
				{
					arInfo = new()
					{
						Language = "ar",
						CreatedOn = DateTime.UtcNow
					};
				}

				var enInfo = location.Info.FirstOrDefault(x => x.Language == "en");

				if (enInfo is null)
				{
					enInfo = new()
					{
						Language = "en",
						CreatedOn = DateTime.UtcNow
					};
				}

				arInfo.Description = Input.DescriptionAr;
				arInfo.SelectionMessage = Input.SelectionMessageAr;
				arInfo.Name = Input.NameAr;
				arInfo.ModifiedOn = DateTime.UtcNow;
				arInfo.ModifiedBy = userId;

				enInfo.Description = Input.DescriptionEn;
				enInfo.SelectionMessage = Input.SelectionMessageEn;
				enInfo.Name = Input.NameEn;
				enInfo.ModifiedOn = DateTime.UtcNow;
				enInfo.ModifiedBy = userId;

				location.Info.Add(arInfo);
				location.Info.Add(enInfo);

				if (Input.Cover != null && Input.Cover.Length > 0)
				{
					location.Cover = await _fileManager.SaveFileAsync(Folder.LocationsCover, Input.Cover);
				}

				if (id.HasValue)
				{
					_db.Locations.Update(location);
				}
				else
				{
					_db.Locations.Add(location);
				}

				await _db.SaveChangesAsync();

				return new LocalRedirectResult("/Admin/Locations");
			}
			else
			{
				ModelState.AddModelError(string.Empty, $"{_localizer["FileTooLarge"]} - 5MB");
			}

			return Page();
		}

		return Page();
	}
}
