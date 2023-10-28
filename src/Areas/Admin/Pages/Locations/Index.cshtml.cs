using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YallaMasar.Data;

namespace YallaMasar.Areas.Admin.Pages.Locations
{
	public class IndexModel : PageModel
	{
		private readonly AppDbContext _db;

		public ICollection<Location> Locations;

		public class Location
		{
			public Guid Id { get; set; }
			public string Name { get; set; }
			public string Cover { get; set; }
		}

		public IndexModel(
			AppDbContext db
		)
		{
			_db = db;
		}

		public void OnGet()
		{
			Locations = _db.Locations
				.Include(location => location.Info)
				.Select(location => new Location
				{
					Cover = location.Cover,
					Id = location.Id,
					Name = location.Info.First(info => info.Language == Thread.CurrentThread.CurrentUICulture.Name).Name
				})
				.ToList();
		}
	}
}
