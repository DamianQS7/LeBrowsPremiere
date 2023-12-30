using LeBrowsPremiere.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LeBrowsPremiere.Models
{
	public class ShoppingCartViewModel
	{
		public IEnumerable<ShoppingCart> CustomerCarts { get; set; }
		public double CartTotal { get; set; }
	}
}
