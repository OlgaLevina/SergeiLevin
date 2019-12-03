using SergeiLevin0.Data;
using SergeiLevin0.Domain.Entities;
using SergeiLevin0.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Infrastructure.Services
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Category> GetCategories() => TestData.Categories;

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            var query = TestData.Products;
            if (filter?.CategoryId != null)
                query = query.Where(product => product.CategoryId == filter.CategoryId);
            if (filter?.BrandId != null)
                query = query.Where(product => product.BrandId == filter.BrandId);
            return query;
        }

        public Product GetProductById(int id) => TestData.Products.FirstOrDefault(p => p.Id == id);

    }
}
