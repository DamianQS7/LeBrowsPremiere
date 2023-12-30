using LeBrowsPremiere.Data;
using LeBrowsPremiere.Entities;
using LeBrowsPremiere.Managers;
using LeBrowsPremiere.Models;
using LeBrowsPremiere.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LeBrowsPremiere.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        private BlogManager _manager;
        public BlogController(AppDbContext dbContext)
        {
            _manager = new BlogManager(dbContext);
        }

        [HttpGet()]
        public async Task<IActionResult> Blogs(int id = 0)
        {
            return View(await _manager.GetBlogsViewModel(id));
        }

        [HttpGet()]
        public async Task<IActionResult> Upsert(int id = 0)
        {
            return View(await _manager.GetBlogViewModel(id));
        }

        [HttpPost()]
        public async Task<IActionResult> Upsert(BlogViewModel viewModel)
        {
            try
            {
                await _manager.Upsert(viewModel);
                TempData["toastr:success"] = BlogResource.UpsertSuccessMessage;
            }
            catch
            {
                TempData["toastr:error"] = BlogResource.UpsertFailMessage;
            }
            return RedirectToAction("Blogs", "Blog");
        }

        [HttpGet()]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _manager.Delete(id);
                TempData["toastr:success"] = BlogResource.DeleteSuccessMessage;
            }
            catch
            {
                TempData["toastr:error"] = BlogResource.DeleteFailMessage;
            }
            return RedirectToAction("Blogs", "Blog");
        }
    }
}
