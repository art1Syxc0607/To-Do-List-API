using System.ComponentModel.DataAnnotations;

namespace To_Do_List_API.DTO.Auth
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; } // одна переменная

        [Required, MinLength(6)]
        public string Password { get; set; }



    }
}
