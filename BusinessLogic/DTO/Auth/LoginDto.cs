using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.Auth
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; } // одна переменная

        [Required, MinLength(6)]
        public string Password { get; set; }



    }
}
