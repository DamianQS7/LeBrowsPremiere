using LeBrowsPremiere.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeBrowsPremiere.Models
{
    public class ProductViewModel
    {
        public Product? Product { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? CategoriesList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? SuppliersList { get; set; }
    }
}
