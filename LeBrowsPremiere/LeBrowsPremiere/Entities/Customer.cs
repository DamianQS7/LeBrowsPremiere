using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeBrowsPremiere.Entities
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your first name.")]
        [DisplayName("First Name")]
        [MaxLength(50, ErrorMessage = "The number of characters used can't be greater than 50.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name.")]
        [DisplayName("Last Name")]
        [MaxLength(50, ErrorMessage = "The number of characters used can't be greater than 50.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email.")]
        public string? Email { get; set; }

        //[Required(ErrorMessage = "Please enter your phone number.")]
        [DisplayName("Phone Number")]
        public string? PhoneNumber { get; set; }

        //[Required(ErrorMessage = "Please enter your address.")]
        public string? Address { get; set; }

        //[Required(ErrorMessage = "Please enter your city.")]
        public string? City { get; set; }

        // One-to-Many relationship, no inverse navigation property.
        //[Required(ErrorMessage = "Please indicate your province.")]
        public int? ProvinceId { get; set; }
        public Province? Province { get; set; }

        //[Required(ErrorMessage = "Please enter your postal code.")]
        [RegularExpression("[A-Z]\\d[A-Z] \\d[A-Z]\\d", ErrorMessage = "The postal code must be in X9X 9X9 Format")]
        [DisplayName("Postal Code")]
        public string? PostalCode { get; set; }

        //[Required(ErrorMessage = "Please enter your password")]
        //public string? Password { get; set; }

    }
}
