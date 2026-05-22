using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.Auth
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; } // одна переменная

        [Required, MinLength(6)]
        public string Password { get; set; }

        //[Compare("Password")]
        //public string ConfirmPassword { get; set; }

        [Required]
        public string Username { get; set;}
    }
}
