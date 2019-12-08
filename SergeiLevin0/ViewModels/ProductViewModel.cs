using SergeiLevin0.Domain.Entities.Base.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.ViewModels
{
    public class ProductViewModel : INamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
    }
}
