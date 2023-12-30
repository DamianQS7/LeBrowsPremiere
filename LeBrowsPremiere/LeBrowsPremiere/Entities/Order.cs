using LeBrowsPremiere.Enumerables;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeBrowsPremiere.Entities
{
	public class Order
	{
		public int Id { get; set; }

		[Required]
		public string UserId { get; set; }

		[ValidateNever]
		[ForeignKey("UserId")]
		public User User { get; set; }

		[Required]
		public OrderStatus Status { get; set; }

		[Required]
		public double OrderTotal { get; set; }

		[Required]
		public DateTime OrderDate { get; set; }

		public DateTime ShippingDate { get; set; }

		public string? TrackingNumber { get; set; }

		public string? SessionId { get; set; }

		public string? PaymentIntentId { get; set; }

		[Required]
		public ICollection<OrderItem> Items { get; set; }
	}
}
