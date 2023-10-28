using System.ComponentModel.DataAnnotations;

using YallaMasar.Data.Models.Users;
using YallaMasar.Enums;

namespace YallaMasar.Data.Models.Providers
{
	public class ProviderEntity : BaseEntity
	{
		[MaxLength(150)]
		public string Name { get; set; }

		[MaxLength(1500)]
		public string Description { get; set; }

		[MaxLength(15)]
		public string PhoneNumber { get; set; }

		[MaxLength(100)]
		public string Email { get; set; }

		public Status Status { get; set; }

		public string UserId { get; set; }

		public ApplicationUser User { get; set; }
	}
}
