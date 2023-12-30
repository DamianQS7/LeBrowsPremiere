using System.ComponentModel.DataAnnotations;

namespace LeBrowsPremiere.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter an email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please enter yur first name.")]
        [StringLength(255)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Please enter yur last name.")]
        [StringLength(255)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Password must be minimum 8 characters and contain at least one uppercase letter, " +
            "one lowercase letter, one digit and one special character")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}
