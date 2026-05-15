using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;


namespace BusinessLogic.Services;

public class TaskService : ITaskService
{
    private ITaskRepository _taskRepository;
    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    async public Task CreateTaskAsync(string? description, string title, int userId)
    {
        var task = new UserTask 
        { 
            Title = title,
            Description = description,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow

        };

        await _taskRepository.CreateTaskAsync(task);
    }

    async public Task<List<UserTask>?> GetTasks(int userId)
    {
        return await _taskRepository.GetTasks(userId);
    }

    async public Task<UserTask?> GetTask(int userId, int taskId)
    {
        var task = await _taskRepository.GetTask(taskId);

        if(task == null || task.UserId !=  userId)
            throw new UnauthorizedAccessException("Это не ваша задача");

        return task;
    }
}

