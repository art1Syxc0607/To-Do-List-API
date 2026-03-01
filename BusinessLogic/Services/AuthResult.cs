namespace BusinessLogic.Services;
public class AuthResult
{
    public bool Success { get; set; }
    public string Error { get; set; }
    public string Token { get; set; }
    public int UserId { get; set; }
    public string Email_login { get; set; }


    public static AuthResult SuccessResult(string token, int userId, string email_login) =>
        new()
        {
            Success = true,
            Token = token,
            UserId = userId,
            Email_login = email_login,

        };

    public static AuthResult ErrorResult(string error) =>
        new() { Success = false, Error = error };
}