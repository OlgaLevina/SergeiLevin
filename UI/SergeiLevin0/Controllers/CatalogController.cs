using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Interfaces;
using SergeiLevin0.Domain.ViewModels;
using Microsoft.Extensions.Logging;

namespace SergeiLevin0.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData ProductData;
        public CatalogController(IProductData productData) { ProductData = productData; }
        public IActionResult Shop(int? categoriId, int? brandId)
        {
            var products = ProductData.GetProducts(new Domain.Entities.ProductFilter
            {
                CategoryId = categoriId,
                BrandId = brandId
            });
            return View(new CatalogViewModel
            {
                CategoryId = categoriId,
                BrandId = brandId,
                Products = products.Select(product => new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Order = product.Order,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Brand=product.Brand.Name
                }).OrderBy(product => product.Order)
            });
        }
        public IActionResult ProductDetails(int id, [FromServices] ILogger<CatalogController> logger)
        {
            var product = ProductData.GetProductById(id);
            if (product is null)
            {
                logger.LogWarning($"Заправшиваемый товар {id} отсутствует в каталоге");
                return NotFound();
            }
            logger.LogInformation($"Товар {id} найден");
            return View(new ProductViewModel
            {
                Id =product.Id,
                Name=product.Name,
                Price=product.Price,
                ImageUrl=product.ImageUrl,
                Order=product.Order,
                Brand=product.Brand?.Name
            });
        }

    }
}