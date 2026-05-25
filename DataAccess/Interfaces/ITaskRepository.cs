using DataAccess.Entities;

namespace DataAccess.Interfaces;

public interface ITaskRepository
{
    Task<List<UserTask>?> GetTasksAsync(int userId);
    Task<UserTask?> GetTaskWithTagsAsync(int taskId);
    Task<UserTask?> GetTaskAsync(int taskId);
    Task CreateTaskAsync(UserTask task);
    Task UpdateTaskAsync(UserTask task);
    Task DeleteTaskAsync(UserTask task);

    Task<UserTask?> GetTaskWithCategoryAsync(int taskId);

    Task<UserTask?> GetTaskWithAllAsync(int taskId);


}

