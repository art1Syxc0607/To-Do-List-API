using BusinessLogic.DTO.Tag;

namespace BusinessLogic.Interfaces;

public interface ITagService
{
    Task<TagResponseDto> CreateAsync(int userId, CreateTagDto dto);
    Task<List<TagResponseDto>> GetUserTagsAsync(int userId);
    Task<TagResponseDto?> GetByIdAsync(int userId, int tagId);
    Task UpdateAsync(int userId, int tagId, UpdateTagDto dto);
    Task DeleteAsync(int userId, int tagId);
}