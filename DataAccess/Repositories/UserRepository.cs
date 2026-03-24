using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;
using DataAccess.Interfaces;


namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppContext _context;

        public UserRepository(AppContext context)
        {
            _context = context;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task CreateAsync(User person)
        {
            _context.Users.Add(person);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByUserNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.UserName == username);
        }
    }
}
