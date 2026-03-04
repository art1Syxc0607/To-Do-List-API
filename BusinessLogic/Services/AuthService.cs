using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        public IUserRepository _userRepository;

        public AuthService(IUserRepository personRepository)
        {
            _userRepository = personRepository;
        }
        public async Task<AuthResult> RegisterAsync(string email, string password)
        {
            // 1. Проверяем, есть ли уже такой пользователь
            var existingPerson = await _userRepository.GetByEmailAsync(email);
            if (existingPerson != null)
                return AuthResult.ErrorResult("Пользователь уже существует");

            // 2. Хешируем пароль
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            //var user = new User

            //await _userRepository.CreateAsync(user);
        }

        //public Task<AuthResult> LoginAsync(string email, string password)
        //{

        //}
    }
}
