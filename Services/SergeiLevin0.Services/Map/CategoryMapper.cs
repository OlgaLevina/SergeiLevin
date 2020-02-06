using SergeiLevin0.Domain.DTO.Products;
using SergeiLevin0.Domain.Entities;

namespace SergeiLevin0.Services.Map
{
    public static class CategoryMapper
    {
        public static CategoryDTO ToDTO(this Category category) => category is null ? null : new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name
        };
        public static Category FromDTO(this CategoryDTO category) => category is null ? null : new Category
        {
            Id = category.Id,
            Name = category.Name
        };
    }
}
