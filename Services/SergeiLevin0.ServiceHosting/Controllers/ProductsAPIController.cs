using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Domain.DTO.Products;
using SergeiLevin0.Domain.Entities;
using SergeiLevin0.Interfaces;

namespace SergeiLevin0.ServiceHosting.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/products")]
    [ApiController]
    public class ProductsAPIController : ControllerBase, IProductData
    {
        private readonly IProductData ProductData;

        public ProductsAPIController(IProductData ProductData) => this.ProductData = ProductData;
        //по умолчанию адрес конечной точки - адрес контроллера / адрес метода - поэтому меняем, как нам надо
        [HttpGet("brands")]
        public IEnumerable<Brand> GetBrands()=> ProductData.GetBrands();
        [HttpGet("brands/{id}")]
        public Brand GetBrandById(int id) => GetBrandById(id);
        [HttpGet("categories")]
        public IEnumerable<Category> GetCategories()=>ProductData.GetCategories();
        [HttpGet("categories/{id}")]
        public Category GetCategoryById(int id) => ProductData.GetCategoryById(id);
        [HttpGet("{id}"), ActionName("Get")]
        public ProductDTO GetProductById(int id)=> ProductData.GetProductById(id);
        [HttpPost, ActionName("Post")]
        public IEnumerable<ProductDTO> GetProducts([FromBody]ProductFilter filter = null)=> ProductData.GetProducts(filter);
    }
}