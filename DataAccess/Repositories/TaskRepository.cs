using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class TaskRepository : ITaskRepository 
{
    private AppContext _context;
    public TaskRepository(AppContext context)
        { _context = context; }

    async public Task<List<UserTask>?> GetTasks(int userId)
    {
        return await _context.Tasks
            .Where(n => n.UserId == userId) // ← Фильтр по пользователю
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }
}

