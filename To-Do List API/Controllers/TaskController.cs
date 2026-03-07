//using BusinessLogic.Services;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using To_Do_List_API.DTO.Auth;

namespace To_Do_List_API.Controllers;

[ApiController]
[Route("/api/auth")]
[Authorize]
public class TaskController() : ControllerBase
{




}