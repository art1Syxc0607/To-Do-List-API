using DataAccess.Entities;
using System;

namespace DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUserNameAsync(string email);
        //Task<User?> GetByIdAsync(int id);
        //Task<bool> EmailExistsAsync(string email);
        Task CreateAsync(User person);
    }
}
