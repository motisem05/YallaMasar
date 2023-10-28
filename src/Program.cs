using System.Globalization;
using System.Reflection;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

using YallaMasar;
using YallaMasar.Configurations;
using YallaMasar.Data;
using YallaMasar.Data.Models.Users;
using YallaMasar.Exceptions;
using YallaMasar.Managers;
using YallaMasar.Managers.Locations;
using YallaMasar.Server.Workers;
using YallaMasar.Services;

using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var configuration = builder.Configuration;

var connectionString = configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
services
	.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

services
	.AddDatabaseDeveloperPageExceptionFilter();

services
	.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();

services
	.AddAuthentication()
	.AddMicrosoftAccount(microsoftOptions =>
	{
		microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"];
		microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
	})
	.AddGoogle(googleOptions =>
	{
		googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
		googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
	});

services.AddMemoryCache();

services.AddLocalization(options => options.ResourcesPath = "Resources");

SmtpConfigurations smtpConfigurations = new()
{
	Host = configuration["Smtp:Host"],
	Port = int.Parse(configuration["Smtp:Port"]),
	Username = configuration["Smtp:Username"],
	FromName = configuration["Smtp:FromName"],
	FromEmail = configuration["Smtp:FromEmail"],
	Password = configuration["Smtp:Password"]
};

services.AddSingleton(smtpConfigurations);

services.AddTransient<IFileManager, FileManager>();
services.AddTransient<ILocationsManager, LocationsManager>();

services.AddSingleton<IEmailSender, EmailSender>();
services.AddSingleton<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, EmailSender>();

services
	.AddRazorPages()
	.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
	.AddDataAnnotationsLocalization(options =>
	{
		options.DataAnnotationLocalizerProvider = (type, factory) =>
		{
			var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
			return factory.Create("SharedResource", assemblyName.Name);
		};
	});

services.AddControllers();

services.Configure<RequestLocalizationOptions>(options =>
{
	CultureInfo[] supportedCultures = new[]
	{
		new CultureInfo("ar"),
		new CultureInfo("en")
	};

	options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "ar");
	options.SupportedCultures = supportedCultures;
	options.RequestCultureProviders = new List<IRequestCultureProvider>
	{
		new CookieRequestCultureProvider()
	};
});

services.AddResponseCaching();

var app = builder.Build();

var supportedCultures = new[] { "ar", "en" };

var localizationOptions = new RequestLocalizationOptions()
	.SetDefaultCulture(supportedCultures[0])
	.AddSupportedCultures(supportedCultures)
	.AddSupportedUICultures(supportedCultures);

localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

Task.Run(async () => await DbInitializer.InitDataBase(app.Services));

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseExceptionHandler(builder =>
{
	builder.Run(async context =>
	{
		IStringLocalizer<SharedResource> localizer = builder
			.ApplicationServices
			.GetRequiredService<IStringLocalizer<SharedResource>>();

		context.Response.StatusCode = StatusCodes.Status500InternalServerError;

		// using static System.Net.Mime.MediaTypeNames;
		context.Response.ContentType = Text.Plain;

		var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

		if (exceptionHandlerPathFeature?.Error is BadRequestException exception)
		{
			string error = exceptionHandlerPathFeature?.Error.Message;

			if (!string.IsNullOrEmpty(localizer.GetString(error)))
			{
				error = localizer[error];
			}

			context.Response.StatusCode = exception.Status;

			await context.Response.WriteAsync(error);
		}
		else
		{

			if (app.Environment.IsDevelopment())
			{
				string error = exceptionHandlerPathFeature?.Error.Message;

				if (!string.IsNullOrEmpty(localizer.GetString(error)))
				{
					error = localizer[error];
				}

				await context.Response.WriteAsync(error);
			}
			else
			{
				await context.Response.WriteAsync(localizer["Error"]);
			}
		}
	});
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.Use(async (context, next) =>
{
	if (!context.Request.Cookies.Any(x => x.Key == ".AspNetCore.Culture"))
	{
		Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar");
	}

	await next(context);
});

app.UseRouting();

app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}"
	);

app.Run();
