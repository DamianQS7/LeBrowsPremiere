using Microsoft.AspNetCore.Mvc;

namespace LeBrowsPremiere.Areas.Customer.Controllers
{
	public class GalleryPageController : Controller
	{
		public IActionResult Gallery()
		{
			return View();
		}
	}
}
