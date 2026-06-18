using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataAccess.Repositories;

public class TaskRepository : ITaskRepository 
{
    private AppContext _context;
    public TaskRepository(AppContext context)
        { _context = context; }

    public async Task CreateTaskAsync(UserTask task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
    }

    async public Task<List<UserTask>?> GetTasksAsync(int userId)
    {
        //return await _context.Tasks
        //    .Where(n => n.UserId == userId) // ← Фильтр по пользователю
        //    .OrderByDescending(n => n.CreatedAt)
        //    .ToListAsync();

        return await _context.Tasks
            .Include(t => t.Tags)        // Загружаем теги (многие-ко-многим)
            //.Include(t => t.Category)    // Загружаем категорию (один-ко-многим)
            //.Include(t => t.User)        // Загружаем пользователя (один-ко-многим)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    async public Task<UserTask?> GetTaskAsync(int taskId)
    {
        return await _context.Tasks
            .Include(t => t.Tags)        // Загружаем теги (многие-ко-многим)
            //.Include(t => t.Category)    // Загружаем категорию (один-ко-многим)
            //.Include(t => t.User)        // Загружаем пользователя (один-ко-многим)
            .FirstOrDefaultAsync(n => n.Id == taskId);
            
    }

    public async Task<UserTask?> GetTaskWithTagsAsync(int taskId)
    {
        return await _context.Tasks
            .Include(t => t.Tags)  // ← загружаем теги
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public async Task UpdateTaskAsync(UserTask task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteTaskAsync(UserTask task)
    {
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }

    public async Task<UserTask?> GetTaskWithCategoryAsync(int taskId)
    {
        return await _context.Tasks
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public async Task<UserTask?> GetTaskWithAllAsync(int taskId)
    {
        return await _context.Tasks
            .Include(t => t.Category)
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public async Task<List<UserTask>?> GetFilteredTasksAsync(int userId, UserTaskStatus? status, Priority? priority, string? search,
         string? dueDateRange, int? categoryId, List<int>? tagsId)
    {

        return await _context.Tasks.Include(t => t.Tags)
            .WhereIf(status != null, t => t.Status == status)
            .WhereIf(priority != null, t => t.Priority == priority)
            .WhereIf(search != null, t => t.Title.Contains(search) || (t.Description != null && 
            t.Description.Contains(search)))

            .WhereIf(categoryId != null, t => t.CategoryId == categoryId)
            .WhereIf(tagsId != null && tagsId.Any(),
    t => tagsId.All(t_Id => t.Tags.Any(tag => tag.Id == t_Id)))
            .ApplyDueDateFilter(dueDateRange)
            .ToListAsync();
    }




}

