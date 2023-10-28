using Microsoft.AspNetCore.Identity;

using YallaMasar.Data;
using YallaMasar.Data.Models.Users;

namespace YallaMasar.Server.Workers
{
    public class DbInitializer
    {
		public static async Task InitDataBase(
			IServiceProvider serviceScopeFactory
		)
		{
			using var scope = serviceScopeFactory.CreateScope();

			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

			var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbInitializer>>();

			ApplicationUser admin = db.Users.FirstOrDefault(x => x.UserName.Contains("Administrator"));

			try
			{
				if (admin == null)
				{
					admin = new()
					{
						UserName = "Administrator",
						Email = configuration["Administrator:Email"]
					};

					var adminCreationResult = await userManager.CreateAsync(admin, configuration["Administrator:Password"] ?? "P@ssw0rd");

					if (adminCreationResult.Succeeded)
					{
						await userManager.ConfirmEmailAsync(admin, await userManager.GenerateEmailConfirmationTokenAsync(admin));


					}
					else
					{
						foreach (var error in adminCreationResult.Errors)
						{
							logger.LogError(error.Description);
						}
					}
				}

				if (!await roleManager.RoleExistsAsync("Administrator"))
				{
					await roleManager.CreateAsync(new IdentityRole("Administrator"));
				}

				if (!await userManager.IsInRoleAsync(admin, "Administrator"))
				{
					await userManager.AddToRoleAsync(admin, "Administrator");
				}

				if (!await roleManager.RoleExistsAsync("Provider"))
				{
					await roleManager.CreateAsync(new IdentityRole("Provider"));
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}
		}
	}
}
