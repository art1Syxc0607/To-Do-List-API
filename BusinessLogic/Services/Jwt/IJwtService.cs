namespace BusinessLogic.Services.Jwt
{
    public interface IJwtService
    {
        string GenerateToken(string email_login, int id);
    }
}
