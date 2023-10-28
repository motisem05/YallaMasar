using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

using YallaMasar.Data;
using YallaMasar.Data.Models.Locations;

namespace YallaMasar.Areas.Locations.Pages
{
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
			public IFormFile Cover { get; set; }

			[Display(Name = "Name")]
			public string Name { get; set; }

			[Display(Name = "Price")]
			public float Price { get; set; }

			[Display(Name = "Age Range")]
			public string AgeRange { get; set; }

			[Display(Name = "Time Needed")]
			public DateTime TimeNeeded { get; set; }

			[Display(Name = "Description")]
			public string Description { get; set; }

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
				.Include(location => location.Info.Where(info => info.Language == Thread.CurrentThread.CurrentUICulture.Name))
				.Include(location => location.InterestingPlaces)
					.ThenInclude(place => place.Info)
				.Include(location => location.Images)
				.FirstAsync(location => location.Id == id);

			CoverPath = location.Cover;

			var info = location.Info.FirstOrDefault();

			if (info != null)
			{
				Input.Description = info.Description;
				Input.Name = info.Name;
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
}
