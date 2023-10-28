using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YallaMasar.Data;
using YallaMasar.Data.Models.Providers;
using YallaMasar.Data.Models.Users;
using YallaMasar.Enums;

namespace YallaMasar.Areas.Identity.Pages.Account.Manage.Trips;

public class AddModel : PageModel
{
	private readonly AppDbContext _db;
	private readonly ILogger<RegisterModel> _logger;

	public AddModel(
		AppDbContext db,
		ILogger<RegisterModel> logger
	)
	{
		_db = db;
		_logger = logger;

		Input = new();
	}

	[TempData]
	public string StatusMessage { get; set; }

	[BindProperty]
	public InputModel Input { get; set; }

	public string ReturnUrl { get; set; }

	public class InputModel
	{
		[Required]
		[Display(Name = "Name")]
		[StringLength(150, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
		public string Name { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Contact Email")]
		public string ContactEmail { get; set; }

		[Required]
		[Display(Name = "Date")]
		[DataType(DataType.Date)]
		public DateTimeOffset Date { get; set; }

		[Required]
		[Display(Name = "Last registration date")]
		[DataType(DataType.Date)]
		public DateTimeOffset LastRegistrationDate { get; set; }

		[Required]
		[Display(Name = "Last withdraw date")]
		[DataType(DataType.Date)]
		public DateTimeOffset LastWithdrawDate { get; set; }

		[Required]
		[Display(Name = "Contact Phone Number")]
		[DataType(DataType.PhoneNumber)]
		public string ContactPhoneNumber { get; set; }

		[Required]
		[Display(Name = "Capacity")]
		public float Capacity { get; set; }

		[Required]
		[Display(Name = "Price")]
		[DataType(DataType.Currency)]
		public float Price { get; set; }

		[Display(Name = "Sale Price")]
		[DataType(DataType.Currency)]
		public float? SalePrice { get; set; }

		[Required]
		[Display(Name = "Description")]
		[StringLength(150, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
		public string Description { get; set; }
	}

	public void OnGet(string returnUrl = null)
	{
		ReturnUrl = returnUrl;

		string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

		ProviderEntity user = _db.Providers.FirstOrDefault(x => x.UserId == userId);

		Input.ContactPhoneNumber = user.PhoneNumber;

		Input.ContactEmail = user.Email;

		Input.Date = DateTimeOffset.Now;
		Input.LastRegistrationDate = DateTimeOffset.Now;
		Input.LastWithdrawDate = DateTimeOffset.Now;
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

			await _db.SaveChangesAsync();

			_db.EndTransaction();

			_logger.LogInformation("[Providers]: {Name} was created and saved to database with id: '{Id}'");

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