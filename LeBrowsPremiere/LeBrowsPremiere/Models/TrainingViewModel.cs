using System.ComponentModel.DataAnnotations;

namespace LeBrowsPremiere.Models
{
    public class TrainingViewModel
    {
        [Required(ErrorMessage = "Please enter your first name.")]
        [MaxLength(50, ErrorMessage = "The number of characters used can't be greater than 50.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name.")]
        [MaxLength(50, ErrorMessage = "The number of characters used can't be greater than 50.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Valid email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        public string? CountryofResidence { get; set; }
    }
}
