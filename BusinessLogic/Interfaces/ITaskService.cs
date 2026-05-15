using BusinessLogic.Services;
using DataAccess.Entities;

namespace BusinessLogic.Interfaces;

public interface ITaskService
{
    Task<List<UserTask>?> GetTasks(int userId);
    Task<UserTask?> GetTask(int userId, int taskId);
    Task CreateTaskAsync(string? description, string title, int userId);
}

