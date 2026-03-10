using Microsoft.AspNetCore.Mvc;
using To_Do_List_API.DTO.Auth;
//using BusinessLogic.Services;
using BusinessLogic.Interfaces;

namespace To_Do_List_API.Controllers;


[ApiController]
[Route("/api/auth")]
public class PersonController(IAuthService authService) : ControllerBase
{
    //public IAuthService _authService { get; set; }
    //public PersonController(IAuthService authService) : base()
    //{

    //}

    [HttpPost("register")]
    async public Task<IActionResult> Register(RegisterDto regDto)
    {
        var result = await authService.RegisterAsync(regDto.Email, regDto.Password);

        // Проверяем флаг успеха
        if (!result.Success)
        {
            // Возвращаем 400 Bad Request с описанием ошибки
            return BadRequest(new { error = result.Error });
        }

        // Успех - возвращаем 200 OK с данными
        var response = new AuthResponseDto
        {
            Token = result.Token,
            UserId = result.UserId,
            EmailLogin = result.Email_login,
            ExpiresIn = 3600
        };

        return Ok(response);

    }


    [HttpPost("login")]
    async public Task<IActionResult> Login(LoginDto loginDto)
    {

    }

}

