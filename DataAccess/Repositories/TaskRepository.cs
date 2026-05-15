using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    async public Task<List<UserTask>?> GetTasks(int userId)
    {
        return await _context.Tasks
            .Where(n => n.UserId == userId) // ← Фильтр по пользователю
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    async public Task<UserTask?> GetTask(int taskId)
    {
        return await _context.Tasks.FirstOrDefaultAsync(n => n.Id == taskId);
    }
}

