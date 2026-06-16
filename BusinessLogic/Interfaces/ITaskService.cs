using BusinessLogic.DTO.Tag;
using BusinessLogic.DTO.UserTasks;
using BusinessLogic.Services;
using DataAccess.Entities;

namespace BusinessLogic.Interfaces;

public interface ITaskService
{
    Task<List<UserTaskResponseDto>?> GetTasksAsync(int userId);
    Task<UserTaskResponseDto?> GetTaskAsync(int userId, int taskId);
    Task CreateTaskAsync(int userId, int? categoryId, ICollection<int> tagsId, string? description, string title, UserTaskStatus Status, 
        Priority Priority, DateTime? DueDate);
    Task UpdateTaskAsync(int userId, int taskId, string? description, string? title, UserTaskStatus? Status,
        Priority? Priority, DateTime? DueDate);
    Task DeleteTaskAsync(int userId, int taskId);


    // === Управление категорией ===
    Task SetCategoryAsync(int userId, int taskId, int categoryId);
    Task RemoveCategoryAsync(int userId, int taskId);

    // === Управление тегами ===
    Task AddTagAsync(int userId, int taskId, int tagId);
    Task RemoveTagAsync(int userId, int taskId, int tagId);
    Task SetTagsAsync(int userId, int taskId, List<int> tagIds);
    Task<List<TagResponseDto>> GetTagsAsync(int userId, int taskId);

    // ========== Фильтрация и Поиск ==========
    Task<List<UserTaskResponseDto>?> GetFilteredTasksAsync(int userId, UserTaskFilterDto taskfilterdto);
}

