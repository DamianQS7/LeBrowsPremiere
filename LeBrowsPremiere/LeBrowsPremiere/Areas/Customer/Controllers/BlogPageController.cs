using LeBrowsPremiere.Data;
using LeBrowsPremiere.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LeBrowsPremiere.Areas.Customer.Controllers
{
    public class BlogPageController : Controller
    {
        private BlogManager _manager;
        public BlogPageController(AppDbContext dbContext)
        {
            _manager = new BlogManager(dbContext);
        }

        
        [HttpGet()]
        public async Task<IActionResult> Blogs(int id = 0)
        {
            return View(await _manager.GetBlogsViewModel(id));
        }
    }
}
