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
    }
}
