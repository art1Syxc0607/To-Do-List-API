using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id);
    Task<List<Category>> GetUserCategoriesAsync(int userId);
    Task<Category> CreateAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Category category);
    Task<bool> ExistsAsync(int id, int userId);
}

