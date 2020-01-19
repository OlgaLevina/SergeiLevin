using SergeiLevin0.Domain.Entities.Base.Interfaces;

namespace SergeiLevin0.Domain.DTO.Products
{
    public class BrandDTO: INamedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
