using LeBrowsPremiere.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LeBrowsPremiere.Models
{
    public class BlogsViewModel
    {
        public List<Blog> Blogs { get; set; } = new();
        public Blog SelectedBlog { get; set; } = new();
        public bool HasSelectedBlog { get { return SelectedBlog.Id > 0; } }
    }
}
