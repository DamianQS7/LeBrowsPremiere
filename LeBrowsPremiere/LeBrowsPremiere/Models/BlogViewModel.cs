using LeBrowsPremiere.Entities;

namespace LeBrowsPremiere.Models
{
    public class BlogViewModel
    {
        public Blog Blog { get; set; } = new();
        public bool IsNew { get { return Blog.Id == 0; } }
    }
}
