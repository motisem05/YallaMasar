using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.DotNet.Scaffolding.Shared;

using YallaMasar.Data;
using YallaMasar.Data.Models.Providers;
using YallaMasar.Data.Models.Users;
using YallaMasar.Enums;
using YallaMasar.Services;

namespace YallaMasar.Areas.Identity.Pages.Account.Manage.Provider
{
	public class RegisterModel : PageModel
	{
		private readonly AppDbContext _db;
		private readonly ILogger<RegisterModel> _logger;

		public RegisterModel(
			AppDbContext db,
			ILogger<RegisterModel> logger
		)
		{
			_db = db;
			_logger = logger;
		}

		[TempData]
		public string StatusMessage { get; set; }

		[BindProperty]
		public InputModel Input { get; set; }

		public string ReturnUrl { get; set; }

		public class InputModel
		{
			[Required]
			[EmailAddress]
			[Display(Name = "Email")]
			public string Email { get; set; }

			[Required]
			[StringLength(150, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
			[Display(Name = "Name")]
			public string Name { get; set; }

			[StringLength(150, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
			[Display(Name = "Description")]
			public string Description { get; set; }

			[Display(Name = "Phone Number")]
			[DataType(DataType.PhoneNumber)]
			public string PhoneNumber { get; set; }
		}

		public void OnGet(string returnUrl = null)
		{
			ReturnUrl = returnUrl;
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/Identity/Account/Manage/Provider");

			if (!ModelState.IsValid)
			{
				return Page();
			}

			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (_db.Providers.Any(provider => provider.UserId == userId))
			{
				StatusMessage = "You already have request under review, please wait untill approved";

				return Page();
			}

			_db.StartTransaction();

			try
			{
				ApplicationUser user = await _db.Users.FindAsync(userId);

				ProviderEntity provider = new()
				{
					Name = Input.Name,
					Description = Input.Description,
					Email = Input.Email,
					PhoneNumber = Input.PhoneNumber,
					Status = Status.WaitingForReview,
					CreatedOn = DateTimeOffset.Now,
					ModifiedOn = DateTimeOffset.Now,
					UserId = userId,
					CreateBy = userId,
					ModifiedBy = userId,
				};

				await _db.Providers.AddAsync(provider);

				await _db.SaveChangesAsync();

				user.ProviderId = provider.Id;

				_db.Users.Update(user);

				await _db.SaveChangesAsync();

				_db.EndTransaction();

				_logger.LogInformation("[Providers]: {Name} was created and saved to database with id: '{Id}'", provider.Name, provider.Id);

				return LocalRedirect(returnUrl);
			}
			catch (Exception ex)
			{
				_db.RollBackTransaction();

				_logger.LogError("[Providers]: {Message}", ex.Message);

				throw;
			}
		}
	}
}
