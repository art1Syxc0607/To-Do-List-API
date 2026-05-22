namespace BusinessLogic.DTO.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int ExpiresIn { get; set; } = 3600;
    }
}
