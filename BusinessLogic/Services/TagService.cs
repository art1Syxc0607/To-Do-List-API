using BusinessLogic.DTO.Tag;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;

namespace BusinessLogic.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;

    public TagService(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<TagResponseDto> CreateAsync(int userId, CreateTagDto dto)
    {
        var tag = new Tag
        {
            Name = dto.Name.Trim(),
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _tagRepository.CreateAsync(tag);

        return new TagResponseDto
        {
            Id = created.Id,
            Name = created.Name,
            CreatedAt = created.CreatedAt,
            TasksCount = 0
        };
    }

    public async Task<List<TagResponseDto>> GetUserTagsAsync(int userId)
    {
        var tags = await _tagRepository.GetUserTagsAsync(userId);

        return tags.Select(t => new TagResponseDto
        {
            Id = t.Id,
            Name = t.Name,
            CreatedAt = t.CreatedAt,
            TasksCount = t.UserTasks?.Count ?? 0
        }).ToList();
    }

    public async Task<TagResponseDto?> GetByIdAsync(int userId, int tagId)
    {
        var tag = await _tagRepository.GetByIdAsync(tagId);

        if (tag == null)
            return null;

        if (tag.UserId != userId)
            throw new UnauthorizedAccessException("Нет доступа к этому тегу");

        return new TagResponseDto
        {
            Id = tag.Id,
            Name = tag.Name,
            CreatedAt = tag.CreatedAt,
            TasksCount = tag.UserTasks?.Count ?? 0
        };
    }

    public async Task UpdateAsync(int userId, int tagId, UpdateTagDto dto)
    {
        var tag = await _tagRepository.GetByIdAsync(tagId);

        if (tag == null)
            throw new InvalidOperationException("Тег не найден");

        if (tag.UserId != userId)
            throw new UnauthorizedAccessException("Нельзя редактировать чужой тег");

        tag.Name = dto.Name.Trim();

        await _tagRepository.UpdateAsync(tag);
    }

    public async Task DeleteAsync(int userId, int tagId)
    {
        var tag = await _tagRepository.GetByIdAsync(tagId);

        if (tag == null)
            throw new InvalidOperationException("Тег не найден");

        if (tag.UserId != userId)
            throw new UnauthorizedAccessException("Нельзя удалить чужой тег");

        await _tagRepository.DeleteAsync(tag);
    }
}