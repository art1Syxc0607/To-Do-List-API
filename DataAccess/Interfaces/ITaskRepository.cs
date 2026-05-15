using DataAccess.Entities;

namespace DataAccess.Interfaces;

public interface ITaskRepository
{
    Task<List<UserTask>?> GetTasks(int userId);
    Task<UserTask?> GetTask(int taskId);
    Task CreateTaskAsync(UserTask task);
    //Task DeleteTaskAsync(int taskId);
    //Task UpdateTaskAsync(Task task);
}

