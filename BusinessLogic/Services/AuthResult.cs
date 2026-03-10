namespace BusinessLogic.Services;
public class AuthResult
{
    public bool Success { get; set; }
    public string Error { get; set; }
    public string Token { get; set; }
    public int UserId { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }


    public static AuthResult SuccessResult(string token, int userId, string email, string username) =>
        new()
        {
            Success = true,
            Token = token,
            UserId = userId,
            Email = email,
            UserName = username
        };

    public static AuthResult ErrorResult(string error) =>
        new() { Success = false, Error = error };
}