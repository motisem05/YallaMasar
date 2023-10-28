using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YallaMasar.Data;
using YallaMasar.Data.Models.Providers;
using YallaMasar.Data.Models.Users;
using YallaMasar.Enums;

namespace YallaMasar.Areas.Admin.Pages.Providers
{
	public class ReviewModel : PageModel
	{
		private readonly ILogger<ReviewModel> _logger;

		private readonly AppDbContext _db;
		private readonly UserManager<ApplicationUser> _userManager;
		public ProviderEntity Provider;

		public ReviewModel(
			ILogger<ReviewModel> logger,
			AppDbContext db,
			UserManager<ApplicationUser> userManager
		)
		{
			_logger = logger;
			_db = db;
			_userManager = userManager;
		}

		public IActionResult OnGet(Guid id)
		{
			Provider = _db.Providers.Find(id);

			if (Provider is null)
			{
				return LocalRedirect("/notfound");
			}

			return Page();
		}

		public async Task<IActionResult> OnPost(Guid id, [FromForm] Status status)
		{
			_db.StartTransaction();

			ProviderEntity entity = _db.Providers.Find(id);
			
			if (entity is null)
			{
				return LocalRedirect("/notfound");
			}

			try
			{
				entity.Status = status;

				_db.Providers.Update(entity);

				ApplicationUser user = await _userManager.FindByIdAsync(entity.UserId);

				if (entity.Status == Status.Approved)
				{
					await _userManager.AddToRoleAsync(user, "Provider");
				}
				else if (status == Status.Rejected && await _userManager.IsInRoleAsync(user, "Provider"))
				{
					await _userManager.RemoveFromRoleAsync(user, "Provider");
				}

				await _db.SaveChangesAsync();

				_db.EndTransaction();

				return LocalRedirect("/Admin/Providers");
			}
			catch (System.Exception ex)
			{
				_db.RollBackTransaction();

				_logger.LogError("[Providers] Failed to review provider '{Name}' due to '{Message}'", entity.Name, ex.Message);

				Provider = entity;

				ModelState.AddModelError(string.Empty, ex.Message);

				return Page();
			}
		}
	}
}
