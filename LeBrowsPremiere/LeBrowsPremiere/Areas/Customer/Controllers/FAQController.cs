using Microsoft.AspNetCore.Mvc;

namespace LeBrowsPremiere.Areas.Customer.Controllers
{
    public class FAQController : Controller
    {
        private readonly ILogger<FAQController> _logger;

        public FAQController(ILogger<FAQController> logger)
        {
            _logger = logger;
        }

        public IActionResult Faqs()
        {
            return View();
        }
    }
}
