using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using YallaMasar.Data;
using YallaMasar.Data.Models.Locations;

namespace YallaMasar.Areas.Admin.Pages.Locations;

public class DetailsModel : PageModel
{
	private readonly AppDbContext _db;
	private readonly IStringLocalizer<SharedResource> _localizer;

	public InputModel Input { get; set; }

	public string CoverPath { get; set; }

	public DetailsModel(
		AppDbContext db,
		IStringLocalizer<SharedResource> localizer
	)
	{
		_db = db ?? throw new ArgumentNullException(nameof(db));
		_localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
		Input = new();
	}

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
		[Display(Name = "Price")]
		public float Price { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[DataType(DataType.Text)]
		[Display(Name = "AgeRange")]
		public string AgeRange { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[DataType(DataType.DateTime)]
		[Display(Name = "TimeNeeded")]
		public DateTime TimeNeeded { get; set; }

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

		[Display(Name = "Images")]
		public List<string> Images { get; set; }

		[Display(Name = "Places")]
		public List<InterestingPlaceEntity> Places { get; set; }
	}

	public Guid Id { get; set; }

	public async Task OnGetAsync(Guid id)
	{
		Id = id;

		await Load(id);
	}

	public async Task Load(Guid id)
	{
		LocationEntity location = await _db.Locations
			.Include(location => location.Info)
			.Include(location => location.InterestingPlaces)
				.ThenInclude(place => place.Info)
			.Include(location => location.Images)
			.FirstAsync(location => location.Id == id);

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

		if (location.Images != null)
		{
			Input.Images = location.Images.Select(x => x.Name).ToList();
		}

		if (location.InterestingPlaces != null)
		{
			Input.Places = location.InterestingPlaces.ToList();
		}
	}
}