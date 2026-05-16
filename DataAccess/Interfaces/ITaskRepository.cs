using DataAccess.Entities;

namespace DataAccess.Interfaces;

public interface ITaskRepository
{
    Task<List<UserTask>?> GetTasksAsync(int userId);
    Task<UserTask?> GetTaskAsync(int taskId);
    Task CreateTaskAsync(UserTask task);
    Task UpdateTaskAsync(UserTask task);
    Task DeleteTaskAsync(UserTask task);

}

