using BusinessLogic.DTO.Category;

namespace BusinessLogic.Interfaces;

public interface ICategoryService
{
    Task<CategoryResponseDto> CreateAsync(int userId, CreateCategoryDto dto);
    Task<List<CategoryResponseDto>> GetUserCategoriesAsync(int userId);
    Task<CategoryResponseDto?> GetByIdAsync(int id, int userId);
    Task UpdateAsync(int userId, int id, UpdateCategoryDto dto);
    Task DeleteAsync(int userId, int id);
}
