using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
