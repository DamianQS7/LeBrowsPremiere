using Microsoft.AspNetCore.Mvc;

namespace LeBrowsPremiere.Areas.Customer.Controllers
{
    public class ServicesPageController : Controller
    {
        public IActionResult Services()
        {
            return View();
        }
    }
}
