using LeBrowsPremiere.Data;
using LeBrowsPremiere.Entities;
using LeBrowsPremiere.Models;
using Microsoft.EntityFrameworkCore;

namespace LeBrowsPremiere.Managers
{
    public class BlogManager
    {
        private AppDbContext _dbContext;
        public BlogManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BlogsViewModel> GetBlogsViewModel(int blogId) {
            var viewModel = new BlogsViewModel();
            var blogs = await _dbContext.Blogs.OrderByDescending(b => b.CreatedDate).ToListAsync();
            if (blogs == null) return viewModel;

            var selectedBlog = blogs.FirstOrDefault();
            if (blogId > 0) selectedBlog = blogs.FirstOrDefault(b => b.Id == blogId);

            viewModel.Blogs = blogs;
            viewModel.SelectedBlog = selectedBlog;
            return viewModel;
        }

        public async Task<BlogViewModel> GetBlogViewModel(int blogId)
        {
            var viewModel = new BlogViewModel();
            var blog = await _dbContext.Blogs.FirstOrDefaultAsync(b => b.Id == blogId);
            if (blog == null) return viewModel;

            viewModel.Blog = blog;
            return viewModel;
        }

        public async Task Upsert(BlogViewModel viewModel)
        {
            var blog = new Blog();
            if (!viewModel.IsNew) blog = await _dbContext.Blogs.FirstOrDefaultAsync(b => b.Id == viewModel.Blog.Id);
            if (blog == null) return;

            blog.Title = viewModel.Blog.Title;
            blog.CreatedDate = DateTime.Now;
            blog.Description = viewModel.Blog.Description;

            if(viewModel.IsNew) _dbContext.Blogs.Add(blog);
            else _dbContext.Blogs.Update(blog);

            _dbContext.SaveChanges();
        }

        public async Task Delete(int blogId)
        {
            var blog = await _dbContext.Blogs.FirstOrDefaultAsync(b => b.Id == blogId);
            if (blog == null) return;

            _dbContext.Blogs.Remove(blog);

            _dbContext.SaveChanges();
        }
    }
}
