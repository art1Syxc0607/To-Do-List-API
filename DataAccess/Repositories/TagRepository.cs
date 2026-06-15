using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class TagRepository : ITagRepository
{
    private readonly AppContext _context;

    public TagRepository(AppContext context)
    {
        _context = context;
    }

    public async Task<List<Tag>> GetUserTagsAsync(int userId)
    {
        return await _context.Tags
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<Tag?> GetByIdAsync(int id)
    {
        return await _context.Tags
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tag> CreateAsync(Tag tag)
    {
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
        return tag;
    }

    public async Task UpdateAsync(Tag tag)
    {
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Tag tag)
    {
        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Tags.AnyAsync(t => t.Id == id);
    }

    public async Task<List<Tag>> GetByIdsAsync(ICollection<int> ids, int userId)
    {
        if (ids == null || ids.Count == 0)
            return new List<Tag>();
        return await _context.Tags
            .Where(t => ids.Contains(t.Id) && t.UserId == userId)
            .ToListAsync();
    }
}