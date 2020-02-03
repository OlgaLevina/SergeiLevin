using SergeiLevin0.Domain.Entities.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.Domain.DTO.Products
{
    public class CategoryDTO : INamedEntity
    {
        public int Id { get; set; }
        public string Name {  get; set;}
    }
}
