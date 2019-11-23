using SergeiLevin0.Domain.Entities.Base;
using SergeiLevin0.Domain.Entities.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.Domain.Entities
{
    public class Product : NamedEntity, IOrderedEntity
    {
        public int Order { get ; set ; }
        public int CategoryId { get; set; }
        public int? BrandId { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}
