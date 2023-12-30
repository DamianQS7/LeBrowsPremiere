using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LeBrowsPremiere.Entities
{
	public class ShoppingCart
	{
		[Key]
		public int Id { get; set; }

		[Range(1, 1000)]
		public int Count { get; set; }

		public int ProductId { get; set; }

		[ForeignKey("ProductId")]
		[ValidateNever]
		public Product Product { get; set; }

		public string UserId { get; set; }

		[ForeignKey("UserId")]
		[ValidateNever]
		public User User { get; set; }

		public double Price { get; set; }
	}
}
