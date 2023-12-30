using System.ComponentModel.DataAnnotations;

namespace LeBrowsPremiere.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
