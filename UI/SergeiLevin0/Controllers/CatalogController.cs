﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Interfaces;
using SergeiLevin0.Domain.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SergeiLevin0.Domain.Entities;

namespace SergeiLevin0.Controllers
{
    public class CatalogController : Controller
    {
        private const string PageSize = "PageSize";
        
        private readonly IProductData ProductData;
        private readonly IConfiguration Configuration;

        public CatalogController(IProductData productData, IConfiguration configuration)
        {
            ProductData = productData;
            Configuration = configuration;
        }
        public IActionResult Shop(int? categoriId, int? brandId, int Page=1)
        {
            var page_size = int.TryParse(Configuration[PageSize], out var size) ? size : (int?)null;
            var products = ProductData.GetProducts(new Domain.Entities.ProductFilter
            {
                CategoryId = categoriId,
                BrandId = brandId,
                Page = Page,
                PageSize = page_size
            });
            return View(new CatalogViewModel
            {
                CategoryId = categoriId,
                BrandId = brandId,
                Products = products.Products.Select(product => new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Order = product.Order,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Brand=product.Brand?.Name
                }).OrderBy(product => product.Order),
                PageViewModel = new PageViewModel
                {
                    PageSize = page_size ?? 0,
                    PageNumber = Page,
                    TotalItems = products.TotalCount
                }
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

        #region API

        public IActionResult GetFilteredItems(int? CategoryId, int? BrandId, int Page)
        {
            var products = GetProducts(CategoryId, BrandId, Page);
            return PartialView("Partial/_FeaturesItem");
        }

        public IEnumerable<ProductViewModel> GetProducts(int? CategoryId, int? BrandId, int Page)
        {
            var products_model = ProductData.GetProducts(new ProductFilter
            {
                CategoryId = CategoryId,
                BrandId = BrandId,
                Page = Page,
                PageSize = int.Parse(Configuration[PageSize])
            });

            return products_model.Products.Select(product => new ProductViewModel
            {
                //потом везде сделать мэрринг!!!
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Order = product.Order,
                Brand = product.Brand?.Name ?? string.Empty,
                ImageUrl = product.ImageUrl
            });
        }

        #endregion

    }
}