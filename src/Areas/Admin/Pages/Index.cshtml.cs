using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace YallaMasar.Areas.Admin.Pages
{
	public class IndexModel : PageModel
	{
		public void OnGet()
		{
			ViewData["Title"] = "Dashboard";
			ViewData["ActivePage"] = ManageAdminNavPages.Index;
		}
	}
}
