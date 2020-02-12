using System.Collections.Generic;

namespace SergeiLevin0.Domain.ViewModels
{
    public class CatalogViewModel
    {
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
