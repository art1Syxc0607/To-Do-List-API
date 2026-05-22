
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppContext _context;

    public CategoryRepository(AppContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetUserCategoriesAsync(int userId)
    {
        return await _context.Categories
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        // Репозиторий просто получает категорию по ID
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    // ... остальные методы
}
