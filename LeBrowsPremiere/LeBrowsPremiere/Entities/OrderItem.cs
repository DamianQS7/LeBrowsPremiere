using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeBrowsPremiere.Entities
{
	public class OrderItem
	{
		public int Id { get; set; }

		[Required]
		public int Quantity { get; set; }

		[Required]
		public double Price { get; set; }

		[Required]
		public int ProductId { get; set; }

		[ValidateNever]
		[ForeignKey("ProductId")]
		public Product Product { get; set; }

		[Required]
		public int OrderId { get; set; }

		[ValidateNever]
		[ForeignKey("OrderId")]
		public Order Order { get; set; }

	}
}
