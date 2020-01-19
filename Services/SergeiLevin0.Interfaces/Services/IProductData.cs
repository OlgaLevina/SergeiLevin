using SergeiLevin0.Domain.DTO.Products;
using SergeiLevin0.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Category> GetCategories();
        IEnumerable<Brand> GetBrands();
        IEnumerable<ProductDTO> GetProducts(ProductFilter filter=null);
        ProductDTO GetProductById(int id);
    }
}
