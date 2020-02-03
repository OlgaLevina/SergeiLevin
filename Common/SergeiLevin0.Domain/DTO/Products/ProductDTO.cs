using SergeiLevin0.Domain.Entities.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.Domain.DTO.Products
{
    public class ProductDTO: INamedEntity, IOrderedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public BrandDTO Brand { get; set; }
        public CategoryDTO Catefory { get; set; }
    }
}
