using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Domain.DTO.Products;
using SergeiLevin0.Domain.ViewModels.BreadCrumbs;
using SergeiLevin0.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductData ProductData;
        public BreadCrumbsViewComponent(IProductData productData) => ProductData = productData;
        public IViewComponentResult Invoke(BreadCrumbsType Type, int id, BreadCrumbsType FromType)
        {
            switch (Type)
            {
                default: return View(Array.Empty<BreadCrumbViewModel>());
                case BreadCrumbsType.Category:
                    return View(new[]
                    {
                        new BreadCrumbViewModel
                        {
                            BreadCrumbsType = BreadCrumbsType.Category,
                            Id = id.ToString(),
                            Name = ProductData.GetCategoryById(id).Name
                        }
                    });
                case BreadCrumbsType.Brand:
                    return View(new[]
                    {
                        new BreadCrumbViewModel
                        {
                            BreadCrumbsType = BreadCrumbsType.Brand,
                            Id = id.ToString(),
                            Name = ProductData.GetBrandById(id).Name
                        }
                    });

                case BreadCrumbsType.Product:
                    return View(GetProductBreadCrumbs(ProductData.GetProductById(id), FromType));
            }
        }

        private IEnumerable<BreadCrumbViewModel> GetProductBreadCrumbs(ProductDTO Product, BreadCrumbsType FromType) => new[]
            {
                new BreadCrumbViewModel
                {
                    BreadCrumbsType = FromType,
                    Id = FromType == BreadCrumbsType.Category
                         ? Product.Category.Id.ToString()
                         : Product.Brand.Id.ToString(),
                    Name = FromType == BreadCrumbsType.Category
                           ? Product.Category.Name
                           : Product.Brand.Name
                },
                new BreadCrumbViewModel
                {
                    BreadCrumbsType = BreadCrumbsType.Product,
                    Id = Product.Id.ToString(),
                    Name = Product.Name
                },
            };
    }
}
