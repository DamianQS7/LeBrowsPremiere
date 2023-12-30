using System.ComponentModel.DataAnnotations;

namespace LeBrowsPremiere.Entities
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required]
        public string? CompanyName { get; set;}

        [Required]
        public string? ContactFirstName { get; set;}

        [Required]
        public string? ContactLastName { get; set; }

        [Required]
        public string? ContactEmail { get; set;}

        [Required]
        public string? ContactPhone { get; set; }

    }
}
