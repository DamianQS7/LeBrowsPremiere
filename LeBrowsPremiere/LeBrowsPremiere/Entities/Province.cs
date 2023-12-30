using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeBrowsPremiere.Entities
{
    public class Province
    {
        [Key]
        public int ProvinceId { get; set; }

        [DisplayName("Province Name")]
        public string? ProvinceName { get;set; }
    }
}
