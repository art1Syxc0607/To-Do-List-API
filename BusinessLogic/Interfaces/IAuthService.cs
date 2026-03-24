using BusinessLogic.Services;

namespace BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(string email, string password, string username);
        Task<AuthResult> LoginAsync(string email, string password);
    }
}
