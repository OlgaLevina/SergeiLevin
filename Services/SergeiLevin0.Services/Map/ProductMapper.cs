using SergeiLevin0.Domain.DTO.Products;
using SergeiLevin0.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.Services.Map
{
    static public class ProductMapper
    {
        public static ProductDTO ToDTO(this Product product) => product is null ? null : new ProductDTO {
            Id = product.Id,
            Name = product.Name,
            ImageUrl=product.ImageUrl,
            Brand=product.Brand.ToDTO(),
            Category=product.Category.ToDTO(),
            Order=product.Order,
            Price=product.Price
        };
        public static Product FromDTO(this ProductDTO product) => product is null ? null : new Product {
            Id = product.Id,
            Name = product.Name,
            ImageUrl = product.ImageUrl,
            Brand = product.Brand.FromDTO(),
            BrandId=product.Brand?.Id,
            Order = product.Order,
            Price = product.Price,
            Category=product.Category.FromDTO()
        };
    }
}
