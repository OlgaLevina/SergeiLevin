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

        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{ServiceAddress}/{id};");

        public IEnumerable<ProductDTO> GetProducts(ProductFilter filter = null) => Post(ServiceAddress, filter).Content.ReadAsAsync<List<ProductDTO>>().Result;
    }
}
