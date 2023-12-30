using LeBrowsPremiere.Entities;
using System.ComponentModel.DataAnnotations;

namespace LeBrowsPremiere.Models
{
    public class ContactFormViewModel
    {
        [Required(ErrorMessage = "Please enter your first name.")]
        [MaxLength(50, ErrorMessage = "The number of characters used can't be greater than 50.")]
        public string ContactFirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name.")]
        [MaxLength(50, ErrorMessage = "The number of characters used can't be greater than 50.")]
        public string ContactLastName { get; set; }

        [Required(ErrorMessage = "Valid email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string ContactEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string ContactPhone { get; set; }

        [Required(ErrorMessage = "You must enter a message.")]
        public string Message { get; set; }
    }
}
