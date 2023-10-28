using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YallaMasar.Data;
using YallaMasar.Enums;

namespace YallaMasar.Areas.Admin.Pages.Providers
{
	public class IndexModel : PageModel
	{
		private readonly AppDbContext _db;

		public ICollection<Provider> Providers;

		public class Provider
		{
			public Guid Id { get; set; }
			public string Name { get; set; }
			public string Email { get; set; }
			public string PhoneNumber { get; set; }
			public Status Status { get; set; }
		}

		public IndexModel(
			AppDbContext db
		)
		{
			_db = db;
		}

		public void OnGet()
		{
			Providers = _db.Providers
				.Select(location => new Provider
				{
					Id = location.Id,
					Name = location.Name,
					Email = location.Email,
					PhoneNumber = location.PhoneNumber,
					Status = location.Status
				})
				.ToList();
		}
	}
}
