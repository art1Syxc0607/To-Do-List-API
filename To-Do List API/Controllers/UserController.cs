using Microsoft.AspNetCore.Mvc;
using To_Do_List_API.DTO.Auth;

namespace To_Do_List_API.Controllers;


[ApiController]
[Route("/api/auth")]
public class PersonController() : ControllerBase
{

    [HttpPost("register")]
    public Task<IActionResult> Register(RegisterDto regDto)
    {

    }

}

