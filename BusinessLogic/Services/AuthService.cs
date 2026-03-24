using BusinessLogic.Interfaces;
using BusinessLogic.Services.Jwt;
using DataAccess.Entities;
using DataAccess.Interfaces;
using System;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository personRepository, IJwtService jwtService )
        {
            _userRepository = personRepository;
            _jwtService = jwtService;
        }
        public async Task<AuthResult> RegisterAsync(string email, string password, string username)
        {
            // 1. Проверяем, есть ли уже такой пользователь
            var existingPerson = await _userRepository.GetByEmailAsync(email);
            if (existingPerson != null)
                return AuthResult.ErrorResult("Пользователь уже существует");

            if(await _userRepository.GetByUserNameAsync(username) != null)
                return AuthResult.ErrorResult("Такое имя пользователя уже используется");

            // 2. Хешируем пароль
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // 3. Создаем пользователя
            var user = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                UserName = username,
            };

            await _userRepository.CreateAsync(user);

            var token = _jwtService.GenerateToken(user.Email, user.UserName, user.Id);

            return AuthResult.SuccessResult(token, user.Id, user.Email, user.UserName);
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            // 1. Ищем пользователя
            var user = await _userRepository.GetByEmailAsync(email);
            if(user == null)
                return AuthResult.ErrorResult("Неверный логин или пароль");

            // 2. Проверяем пароль
            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isValid)
                return AuthResult.ErrorResult("Неверный логин или пароль");

            var token = _jwtService.GenerateToken(user.Email, user.UserName, user.Id);

            return AuthResult.SuccessResult(token, user.Id, user.Email, user.UserName);

        }
    }
}
