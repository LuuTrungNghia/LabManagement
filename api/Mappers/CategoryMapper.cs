using api.Dtos.Device;
using api.Models;

namespace api.Mappers
{
    public static class CategoryMapper
    {
        public static Category ToCategory(this CreateCategoryDto dto) => new Category
        {
            CategoryName = dto.CategoryName
        };

        public static CategoryDto ToCategoryDto(this Category category) => new CategoryDto
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName
        };
    }
}
