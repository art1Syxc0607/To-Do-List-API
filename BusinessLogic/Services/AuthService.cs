using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        //public 
        public Task<AuthResult> RegisterAsync(string emailLogin, string password)
        {
            // 1. Проверяем, есть ли уже такой пользователь
            var existingPerson = await _personRepository.GetByEmailLoginAsync(emailLogin);
            if (existingPerson != null)
                return AuthResult.ErrorResult("Пользователь уже существует");

            // 2. Хешируем пароль
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
