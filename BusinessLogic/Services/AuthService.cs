using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Repositories;
using BusinessLogic.Services.Jwt;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        public IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository personRepository, IJwtService jwtService )
        {
            _userRepository = personRepository;
            _jwtService = jwtService;
        }
        public async Task<AuthResult> RegisterAsync(string email, string password)
        {
            // 1. Проверяем, есть ли уже такой пользователь
            var existingPerson = await _userRepository.GetByEmailAsync(email);
            if (existingPerson != null)
                return AuthResult.ErrorResult("Пользователь уже существует");

            // 2. Хешируем пароль
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // 3. Создаем пользователя
            var user = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            var token = _jwtService.GenerateToken(user.Email, user.Id);

            return AuthResult.SuccessResult(token, user.Id, user.Email);
        }

        //public Task<AuthResult> LoginAsync(string email, string password)
        //{

        //}
    }
}
