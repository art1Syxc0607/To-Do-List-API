namespace BusinessLogic.Services.Jwt
{
    public interface IJwtService
    {
        string GenerateToken(string email_login, string username,int id);
    }
}
