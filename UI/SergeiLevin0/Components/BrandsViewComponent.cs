using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Interfaces;
using SergeiLevin0.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Components
{
    public class BrandsViewComponent: ViewComponent
    {
        private readonly IProductData ProductData;
        public BrandsViewComponent(IProductData productData) => ProductData = productData;
        public IViewComponentResult Invoke() => View(GetBrands());
        private IEnumerable<BrandViewModel> GetBrands() =>
            ProductData.GetBrands().Select(brand => new BrandViewModel
            {
                Id = brand.Id,
                Name = brand.Name,
                Order = brand.Order
            }).OrderBy(brand =>brand.Order).ToList();
    }
}
