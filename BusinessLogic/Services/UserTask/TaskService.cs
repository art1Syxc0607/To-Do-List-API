using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;


namespace BusinessLogic.Services;

public class TaskService : ITaskService
{
    private ITaskRepository _taskRepository;
    private readonly ICategoryRepository _categoryRepository; 
    public TaskService(ITaskRepository taskRepository, ICategoryRepository categoryRepository)
    {
        _taskRepository = taskRepository;
        _categoryRepository = categoryRepository;
    }

    async public Task CreateTaskAsync(int userId, int? categoryId, string? description, string title, UserTaskStatus status,
        Priority priority, DateTime? dueDate)
    {
        var task = new UserTask 
        { 
            Title = title,
            Description = description,
            UserId = userId,
            CategoryId = categoryId,
            Priority = priority,
            Status = status,
            DueDate = dueDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsCompleted = false,

        };

        await _taskRepository.CreateTaskAsync(task);
    }

    async public Task UpdateTaskAsync(int userId, int taskId, string? description, string? title, UserTaskStatus? status,
       Priority? priority, DateTime? dueDate)
    {
        var task = await _taskRepository.GetTaskAsync(taskId);

        if(task == null)
            throw new InvalidOperationException("Такой задачи нет");

        if (task.UserId != userId)
            throw new UnauthorizedAccessException("Это не ваша задача");

        if (description != null)
            task.Description = description;

        // Обновляем только то, что пришло
        if (title != null)
            task.Title = title;
        if (status.HasValue)           // ← проверка, что значение есть
            task.Status = status.Value;
        task.DueDate = dueDate;
        if (priority.HasValue)
            task.Priority = priority.Value;
        task.UpdatedAt = DateTime.UtcNow;

        await _taskRepository.UpdateTaskAsync(task);
    }

    async public Task DeleteTaskAsync(int userId, int taskId)
    {
        var task = await _taskRepository.GetTaskAsync(taskId);

        if (task == null || task.UserId != userId)
            throw new UnauthorizedAccessException("Это не ваша заметка или такой заметки нет");

        await _taskRepository.DeleteTaskAsync(task);
    }

    async public Task<List<UserTask>?> GetTasks(int userId)
    {
        return await _taskRepository.GetTasksAsync(userId);
    }

    async public Task<UserTask?> GetTask(int userId, int taskId)
    {
        var task = await _taskRepository.GetTaskAsync(taskId);

        if(task == null || task.UserId !=  userId)
            throw new UnauthorizedAccessException("Это не ваша задача");

        return task;
    }
}

