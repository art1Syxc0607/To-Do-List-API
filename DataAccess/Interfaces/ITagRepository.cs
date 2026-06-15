using DataAccess.Entities;

namespace DataAccess.Interfaces;

public interface ITagRepository
{
    Task<List<Tag>> GetUserTagsAsync(int userId);
    Task<Tag?> GetByIdAsync(int id);
    Task<Tag> CreateAsync(Tag tag);
    Task UpdateAsync(Tag tag);
    Task DeleteAsync(Tag tag);
    Task<bool> ExistsAsync(int id);
    Task<List<Tag>> GetByIdsAsync(ICollection<int> ids, int userId);
}