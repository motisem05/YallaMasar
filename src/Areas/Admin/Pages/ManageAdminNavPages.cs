
using Microsoft.AspNetCore.Mvc.Rendering;

namespace YallaMasar.Areas.Admin.Pages;

public static class ManageAdminNavPages
{
	public static string Index => "Index";

	public static string Locations => "Locations";
	
	public static string Providers => "Providers";

	public static string PageNavClass(ViewContext viewContext, string page)
	{
		var activePage = viewContext.ViewData["ActivePage"] as string ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);

		return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "bg-primary-900 bg-opacity-30 backdrop-blur-sm text-white" : null;
	}
}