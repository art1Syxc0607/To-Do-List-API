using BusinessLogic.Services;
using DataAccess.Entities;

namespace BusinessLogic.Interfaces;

public interface ITaskService
{
    Task<List<UserTask>?> GetTasks(int userId);
    Task<UserTask?> GetTask(int userId, int taskId);
    Task CreateTaskAsync(int userId, int? categoryId, int? tagId, string? description, string title, UserTaskStatus Status, 
        Priority Priority, DateTime? DueDate);
    Task UpdateTaskAsync(int userId, int taskId, int? categoryId, int? tagId, string? description, string? title, UserTaskStatus? Status,
        Priority? Priority, DateTime? DueDate);
    Task DeleteTaskAsync(int userId, int taskId);
}

