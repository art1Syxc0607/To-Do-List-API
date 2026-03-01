using System.ComponentModel.DataAnnotations;

namespace To_Do_List_API.DTO.Auth
{
    public class RegisterDto
    {
        [Required]
        public string EmailLogin { get; set; } // одна переменная

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
