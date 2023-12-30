using LeBrowsPremiere.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeBrowsPremiere.Models
{
	public class OrderSummaryViewModel
	{
		[ValidateNever]
		public IEnumerable<ShoppingCart> CustomerCarts { get; set; }

		[ValidateNever]
		public Order NewOrder { get; set; }
		
		[ValidateNever] 
		public User User { get; set; }
		
		[ValidateNever]
		public double CartTotal { get; set; }
		
		[Required]
		public string PhoneNumber { get; set; }
		
		[Required]
		public string Address { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public int ProvinceId { get; set; }

		[RegularExpression("[A-Z]\\d[A-Z] \\d[A-Z]\\d", ErrorMessage = "The postal code must be in X9X 9X9 Format")]
		[Required]
		public string? PostalCode { get; set; }

		[ValidateNever]
		public IEnumerable<SelectListItem>? ProvincesList { get; set; }
	}
}
