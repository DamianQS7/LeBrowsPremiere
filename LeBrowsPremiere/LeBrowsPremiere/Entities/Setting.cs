using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LeBrowsPremiere.Entities
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(50)]
        [Required]
        public string Code { get; set; } = string.Empty;

        [Column(TypeName = "VARCHAR")]
        [MaxLength(250)]
        public string? Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
