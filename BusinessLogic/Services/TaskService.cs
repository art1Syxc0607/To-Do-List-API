using BusinessLogic.DTO.Tag;
using BusinessLogic.DTO.UserTasks;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using System.Threading.Tasks;


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

    async public Task CreateTaskAsync(int userId, int? categoryId, ICollection<int> tagIds,
        string? description, string title, UserTaskStatus status,
        Priority priority, DateTime? dueDate)
    {
        // 1. Валидация обязательных полей
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Название задачи не может быть пустым");

        // 2. Проверка категории
        if (categoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId.Value);

            if (category == null)
                throw new InvalidOperationException($"Категория с ID {categoryId} не существует");

            if (category.UserId != userId)
                throw new UnauthorizedAccessException("Нет доступа к указанной категории");
        }

        // 3. Проверка тегов
        var validTags = new List<Tag>();
        if (tagIds != null && tagIds.Any())
        {
            validTags = await _tagRepository.GetByIdsAsync(tagIds, userId);

            if (validTags.Count != tagIds.Count)
            {
                var validTagIds = validTags.Select(t => t.Id).ToHashSet();
                var invalidTagIds = tagIds.Where(id => !validTagIds.Contains(id)).ToList();
                throw new InvalidOperationException($"Теги с ID {string.Join(", ", invalidTagIds)} не существуют или недоступны");
            }
        }

        // Проверка статуса
        if (!Enum.IsDefined(typeof(UserTaskStatus), status))
            throw new InvalidOperationException($"Недопустимый статус: {status}");

        // Проверка приоритета
        if (!Enum.IsDefined(typeof(Priority), priority))
            throw new InvalidOperationException($"Недопустимый приоритет: {priority}");


        // 4. Создание задачи
        var task = new UserTask
        {
            Title = title.Trim(),
            Description = description?.Trim(),
            UserId = userId,
            CategoryId = categoryId,
            Tags = validTags,
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

    async public Task<List<UserTaskResponseDto>?> GetTasksAsync(int userId)
    {
        var tasks = await _taskRepository.GetTasksAsync(userId);

        if (tasks == null || !tasks.Any())
            return new List<UserTaskResponseDto>();

        return tasks.Select(x => new UserTaskResponseDto         
        {
            Title = x.Title,
            Description = x.Description,
            Id = x.Id,
            UserId = userId,
            CategoryId = x.CategoryId,
            CreateTime = x.CreatedAt,
            UpdateTime = x.UpdatedAt,
            IsCompleted = x.IsCompleted,
            Status = x.Status,
            Priority = x.Priority,
            DueDate = x.DueDate,
            Tags = x.Tags.Select(t => new TagResponseDto
            {
                Id = t.Id,
                Name = t.Name,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                //UserId = userId,

            }).ToList()

        }).ToList();
    }

    async public Task<UserTaskResponseDto?> GetTaskAsync(int userId, int taskId)
    {
        var task = await _taskRepository.GetTaskAsync(taskId);

        if(task == null || task.UserId !=  userId)
            throw new UnauthorizedAccessException("Это не ваша задача");

        var taskResponse = new UserTaskResponseDto
        {
            Title = task.Title,
            Description = task.Description,
            Id = task.Id,
            UserId = userId,
            CategoryId = task.CategoryId,
            CreateTime = task.CreatedAt,
            UpdateTime = task.UpdatedAt,
            IsCompleted = task.IsCompleted,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            Tags = task.Tags.Select(t => new TagResponseDto
            {
                Id = t.Id,
                Name = t.Name,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                //UserId = userId,

            }).ToList()
        };

        return taskResponse;
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

    public async Task<List<TagResponseDto>> GetTagsAsync(int userId, int taskId)
    {
        var task = await _taskRepository.GetTaskWithTagsAsync(taskId);
        if (task == null || task.UserId != userId)
            throw new UnauthorizedAccessException("Нет доступа к задаче");

        return task.Tags.Select(t => new TagResponseDto
        {
            Id = t.Id,
            Name = t.Name,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            UserId = userId,

        }).ToList();
    }

    // ========== Фильтрация и Поиск ==========

    async public Task<List<UserTaskResponseDto>?> GetFilteredTasksAsync(int userId, UserTaskFilterDto taskfilterdto)
    {
        if (taskfilterdto.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(taskfilterdto.CategoryId.Value);

            if (category == null)
                throw new InvalidOperationException($"Category with id {taskfilterdto.CategoryId} not found");

            if (category.UserId != userId)
                throw new UnauthorizedAccessException($"No access to category {category.Id}");
        }


        if (taskfilterdto.TagIds != null)
            foreach (var tagId in taskfilterdto.TagIds)
            {
                var tag = await _tagRepository.GetByIdAsync(tagId);
                if (tag == null || tag.UserId != userId)
                    throw new UnauthorizedAccessException($"Тег {tagId} недоступен");
            }


        var tasks = await _taskRepository.GetFilteredTasksAsync(userId, taskfilterdto.Status, taskfilterdto.Priority,
            taskfilterdto.Search, taskfilterdto.DueDateRange, taskfilterdto.CategoryId, taskfilterdto.TagIds);

        return tasks.Select(x => new UserTaskResponseDto
        {
            Title = x.Title,
            Description = x.Description,
            Id = x.Id,
            UserId = userId,
            CategoryId = x.CategoryId,
            CreateTime = x.CreatedAt,
            UpdateTime = x.UpdatedAt,
            IsCompleted = x.IsCompleted,
            Status = x.Status,
            Priority = x.Priority,
            DueDate = x.DueDate,
            Tags = x.Tags.Select(t => new TagResponseDto
            {
                Id = t.Id,
                Name = t.Name,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                //UserId = userId,

            }).ToList()

        }).ToList();
    }

}

