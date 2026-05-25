using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;


namespace BusinessLogic.Services;

public class TaskService : ITaskService
{
    private ITaskRepository _taskRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITagRepository _tagRepository;
    public TaskService(ITaskRepository taskRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
    {
        _taskRepository = taskRepository;
        _categoryRepository = categoryRepository;
        _tagRepository = tagRepository;
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


    // ========== УПРАВЛЕНИЕ КАТЕГОРИЕЙ ==========

    public async Task SetCategoryAsync(int userId, int taskId, int categoryId)
    {
        // 1. Проверяем задачу
        var task = await _taskRepository.GetTaskAsync(taskId);
        if (task == null || task.UserId != userId)
            throw new UnauthorizedAccessException("Нет доступа к задаче");

        // 2. Проверяем категорию
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null || category.UserId != userId)
            throw new UnauthorizedAccessException("Нет доступа к категории");

        // 3. Устанавливаем
        task.CategoryId = categoryId;
        task.UpdatedAt = DateTime.UtcNow;

        await _taskRepository.UpdateTaskAsync(task);
    }


    public async Task RemoveCategoryAsync(int userId, int taskId)
    {
        var task = await _taskRepository.GetTaskAsync(taskId);
        if (task == null || task.UserId != userId)
            throw new UnauthorizedAccessException("Нет доступа к задаче");

        task.CategoryId = null;
        task.UpdatedAt = DateTime.UtcNow;

        await _taskRepository.UpdateTaskAsync(task);
    }

    // ========== УПРАВЛЕНИЕ ТЕГАМИ ==========

    public async Task AddTagAsync(int userId, int taskId, int tagId)
    {
        // ✅ Загружаем задачу ВМЕСТЕ с тегами
        var task = await _taskRepository.GetTaskWithTagsAsync(taskId);
        if (task == null || task.UserId != userId)
            throw new UnauthorizedAccessException("Нет доступа к задаче");

        var tag = await _tagRepository.GetByIdAsync(tagId);
        if (tag == null || tag.UserId != userId)
            throw new UnauthorizedAccessException("Нет доступа к тегу");

        // ✅ Теперь task.Tags загружен! Работает безопасно
        if (!task.Tags.Any(t => t.Id == tagId))
        {
            task.Tags.Add(tag);
            task.UpdatedAt = DateTime.UtcNow;
            await _taskRepository.UpdateTaskAsync(task);
        }
    }


    public async Task RemoveTagAsync(int userId, int taskId, int tagId)
    {
        var task = await _taskRepository.GetTaskWithTagsAsync(taskId);
        if (task == null || task.UserId != userId)
            throw new UnauthorizedAccessException("Нет доступа к задаче");

        var tag = task.Tags.FirstOrDefault(t => t.Id == tagId);
        if (tag != null)
        {
            task.Tags.Remove(tag);
            task.UpdatedAt = DateTime.UtcNow;
            await _taskRepository.UpdateTaskAsync(task);
        }
    }

    public async Task SetTagsAsync(int userId, int taskId, List<int> tagIds)
    {
        var task = await _taskRepository.GetTaskWithTagsAsync(taskId);
        if (task == null || task.UserId != userId)
            throw new UnauthorizedAccessException("Нет доступа к задаче");

        // Проверяем все теги
        var tags = new List<Tag>();
        foreach (var tagId in tagIds)
        {
            var tag = await _tagRepository.GetByIdAsync(tagId);
            if (tag == null || tag.UserId != userId)
                throw new UnauthorizedAccessException($"Тег {tagId} недоступен");
            tags.Add(tag);
        }

        // Заменяем коллекцию
        task.Tags.Clear();
        foreach (var tag in tags)
            task.Tags.Add(tag);

        task.UpdatedAt = DateTime.UtcNow;
        await _taskRepository.UpdateTaskAsync(task);
    }

    public async Task<List<Tag>> GetTagsAsync(int userId, int taskId)
    {
        var task = await _taskRepository.GetTaskWithTagsAsync(taskId);
        if (task == null || task.UserId != userId)
            throw new UnauthorizedAccessException("Нет доступа к задаче");

        return task.Tags.ToList();
    }

}

