using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeBrowsPremiere.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "The Product price cannot be 0")]
        public double Price { get; set;}

        [Required]
        public string? Brand { get; set;}

        [Required]
        [DisplayName("Current Stock")]
        public int CurrentStock { get; set;}

        [Required]
        [DisplayName("Minimum Stock")]
        public int MinimumStock { get; set;}

        [ValidateNever]
        [DisplayName("Image File")]
        public string? ImageUrl { get; set;}

        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set;}

        [ValidateNever]
        public Category Category { get; set;}

        [Required]
        [DisplayName("Supplier")]
        public int SupplierId { get; set;}

        [ValidateNever]
        public Supplier Supplier { get; set;}

    }
}
