using Microsoft.Extensions.Configuration;
using SergeiLevin0.Clients.Base;
using SergeiLevin0.Domain.DTO.Products;
using SergeiLevin0.Domain.Entities;
using SergeiLevin0.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SergeiLevin0.Clients.Products
{
    public class ProductsClient: BaseClient, IProductData
    {
        public ProductsClient(IConfiguration configuration) : base(configuration, "api/products") { }

        public IEnumerable<Brand> GetBrands() => Get<List<Brand>>($"{ServiceAddress}/brands");//впоследствии здесь тоже будет дто

        public IEnumerable<Category> GetCategories() => Get<List<Category>>($"{ServiceAddress}/categories");

        public Category GetCategoryById(int id) => Get<Category>($"{ServiceAddress}/categories/{id}");

        public Brand GetBrandById(int id) => Get<Brand>($"{ServiceAddress}/brands/{id}");

        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{ServiceAddress}/{id}");

        public PagedProductDTO GetProducts(ProductFilter filter = null) => Post(ServiceAddress, filter).Content.ReadAsAsync<PagedProductDTO>().Result;
    }
}
