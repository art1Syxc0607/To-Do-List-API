
using DataAccess.Entities;
using System;

namespace DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailLoginAsync(string emailLogin);
        Task<User?> GetByIdAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task CreateAsync(User person);
    }
}
