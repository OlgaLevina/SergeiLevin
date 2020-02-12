using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Domain.Entities;
using SergeiLevin0.Interfaces;
using SimpleMvcSitemap;

namespace SergeiLevin0.Controllers
{
    public class SitemapController : Controller
    {
        public IActionResult Index([FromServices] IProductData ProductData)
        {
            var nodes = new List<SitemapNode>//создаем узлы
            {
                new SitemapNode(Url.Action("Index", "Home")),
                new SitemapNode(Url.Action("ContactUs", "Home")),
                new SitemapNode(Url.Action("Blog", "Home")),
                new SitemapNode(Url.Action("BlogSingle", "Home")),
                new SitemapNode(Url.Action("Shop", "Catalog")),
                new SitemapNode(Url.Action("Index", "WebAPITest")),
            };
            nodes.AddRange(ProductData.GetCategories().Select(category => new SitemapNode(Url.Action("Shop", "Catalog", new { CategoryId = category.Id }))));
            //второй способ добавления через форыч
            foreach (var brand in ProductData.GetBrands())  nodes.Add(new SitemapNode(Url.Action("Shop", "Catalog", new { BrandId = brand.Id})));
            foreach (var product in ProductData.GetProducts(new ProductFilter()).Products)  nodes.Add(new SitemapNode(Url.Action("Details", "Catalog", new { product.Id })));
            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));//создаем на основе узлов провайдер карты сайта и саму карту
        }
    }
}