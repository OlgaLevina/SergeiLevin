using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Domain.DTO.Products;
using SergeiLevin0.Domain.ViewModels.BreadCrumbs;
using SergeiLevin0.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SergeiLevin0.Controllers;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductData ProductData;
        public BreadCrumbsViewComponent(IProductData productData) => ProductData = productData;
        //public IViewComponentResult Invoke(BreadCrumbsType Type, int id, BreadCrumbsType FromType)
        public IViewComponentResult Invoke()
        {
            GetParameters(out var Type, out var id, out var FromType);
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

        private void GetParameters(out BreadCrumbsType Type, out int id, out BreadCrumbsType FromType)//вынесли подготовку данных из представления в метод
        {
            if (Request.Query.ContainsKey("CtegoryId")) Type = BreadCrumbsType.Category;
            else
                Type = Request.Query.ContainsKey("BrandId")
                    ? BreadCrumbsType.Brand
                    : BreadCrumbsType.None;
            if ((string)ViewContext.RouteData.Values["action"] == nameof(CatalogController.ProductDetails))
                Type = BreadCrumbsType.Product;
            id = 0;
            FromType = BreadCrumbsType.Category;
            switch (Type)
            {
                default: throw new ArgumentOutOfRangeException();
                case BreadCrumbsType.None: break;
                case BreadCrumbsType.Category:
                    id = int.Parse(Request.Query["CategoryId"].ToString());
                    break;
                case BreadCrumbsType.Brand:
                    id = int.Parse(Request.Query["BrandId"].ToString());
                    break;
                case BreadCrumbsType.Product:
                    id = int.Parse(ViewContext.RouteData.Values["id"].ToString());
                    if (Request.Query.ContainsKey("FromBrand")) FromType = BreadCrumbsType.Brand;
                    break;
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
