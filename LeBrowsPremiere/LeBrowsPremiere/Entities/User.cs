using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LeBrowsPremiere.Entities
{
    public class User : IdentityUser
    {
		[DisplayName("Phone Number")]
		public string? PhoneNumber { get; set; }

		public string? Address { get; set; }

		public string? City { get; set; }

		[RegularExpression("[A-Z]\\d[A-Z] \\d[A-Z]\\d", ErrorMessage = "The postal code must be in X9X 9X9 Format")]
		[DisplayName("Postal Code")]
		public string? PostalCode { get; set; }

		public int? ProvinceId { get; set; }

		public Province? Province { get; set; }
	}
}
