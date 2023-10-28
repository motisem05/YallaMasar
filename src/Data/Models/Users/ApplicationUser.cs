using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

using YallaMasar.Data.Models.Providers;
using YallaMasar.Enums;

namespace YallaMasar.Data.Models.Users;

public class ApplicationUser : IdentityUser
{
	[MaxLength(150)]
	public string FirstName { get; set; }

	[MaxLength(150)]
	public string LastName { get; set; }

	public DateTime? DateOfBirth { get; set; }

	public Gender Gender { get; set; }

	public Guid? ProviderId { get; set; }
	
	public ProviderEntity Provider { get; set; }
}
