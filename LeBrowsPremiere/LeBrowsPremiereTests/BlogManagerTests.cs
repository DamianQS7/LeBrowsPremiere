using LeBrowsPremiere.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using LeBrowsPremiere.Managers;
using LeBrowsPremiere.Models;
using LeBrowsPremiere.Entities;

namespace LeBrowsPremiereTests
{
    public class BlogManagerTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new AppDbContext(options);
            return dbContext;
        }

        [Fact]
        public async Task Upsert_InsertsNewBlog()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var blogManager = new BlogManager(dbContext);
            var newBlogViewModel = new BlogViewModel
            {
                Blog = new Blog
                {
                    Title = "Test Title",
                    Description = "Test Description"
                }
            };

            // Act
            await blogManager.Upsert(newBlogViewModel);
            var insertedBlog = await dbContext.Blogs.FirstOrDefaultAsync(b => b.Title == "Test Title");

            // Assert
            Assert.NotNull(insertedBlog);
            Assert.Equal("Test Title", insertedBlog.Title);
            Assert.Equal("Test Description", insertedBlog.Description);
        }



        [Fact]
        public async Task Delete_RemovesBlog()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var blogManager = new BlogManager(dbContext);
            int blogIdToDelete = 1; // Use an existing blog ID

            // Act
            await blogManager.Delete(blogIdToDelete);
            var deletedBlog = await dbContext.Blogs.FirstOrDefaultAsync(b => b.Id == blogIdToDelete);

            // Assert
            Assert.Null(deletedBlog);
        }

    }

}
