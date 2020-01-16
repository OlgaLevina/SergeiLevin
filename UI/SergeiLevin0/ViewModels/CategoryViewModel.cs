using SergeiLevin0.Domain.Entities.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.ViewModels
{
    public class CategoryViewModel : INamedEntity, IOrderedEntity
    {
        public int Order { get; set ; }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CategoryViewModel> ChildCategories { get; set; } = new List<CategoryViewModel>();
        public CategoryViewModel ParentCategory { get; set; }
    }
}
