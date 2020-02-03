using SergeiLevin0.Domain.DTO.Products;
using SergeiLevin0.Domain.Entities;

namespace SergeiLevin0.Services.Map
{
    static public class BrandMapper
    {
        public static BrandDTO ToDTO(this Brand brand) => brand is null ? null : new BrandDTO {Id = brand.Id,Name = brand.Name};
        public static Brand FromDTO(this BrandDTO brand) => brand is null ? null : new Brand { Id = brand.Id, Name = brand.Name };
    }
}
