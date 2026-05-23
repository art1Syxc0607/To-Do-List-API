//using BusinessLogic.Services;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BusinessLogic.DTO.UserTasks;

namespace To_Do_List_API.Controllers;

[ApiController]
//[Route("/api/auth")]
[Authorize]
public class TaskController(ITaskService _taskService) : ControllerBase
{
    // Метод для получения PersonId
    private int GetCurrentPersonId()
    {
        // User - это свойство ControllerBase, заполненное из токена
        var claim = User.FindFirst("personId") ?? User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
            throw new UnauthorizedAccessException("Пользователь не аутентифицирован");

        return int.Parse(claim.Value);
    }

    [HttpGet("/api/tasks")]
    async public Task<ActionResult<List<UserTaskResponseDto>>> GetTasks()
    {
        var userId = GetCurrentPersonId();
        var tasks = await _taskService.GetTasks(userId);
        return Ok(tasks);
    }

    [HttpGet("/api/tasks{taskId:int}")]
    async public Task<ActionResult<UserTaskResponseDto>> GetTask(int taskId)
    {
        var userId = GetCurrentPersonId();
        var task = await _taskService.GetTask(userId, taskId);

        if (task == null)
            return NotFound();  // ← 404, а не 200

        return Ok(task);
    }

    [HttpPost("/api/tasks")]
    async public Task<IActionResult> CreateTaskAsync([FromBody] CreateTaskDto createdto)
    {
        var userId = GetCurrentPersonId();
        await _taskService.CreateTaskAsync(userId, createdto.CategoryId, createdto.Description, createdto.Title, createdto.Status, 
            createdto.Priority, createdto.DueDate);
        return NoContent();
    }

    [HttpPut("/api/tasks/{taskId:int}")]
    async public Task<IActionResult> UpdateTaskAsync(int taskId, [FromBody] UpdateTaskDto updatedto)
    {
        var userId = GetCurrentPersonId();
        await _taskService.UpdateTaskAsync(userId, taskId, updatedto.CategoryId, updatedto.Description, updatedto.Title,
            updatedto.Status, updatedto.Priority, updatedto.DueDate);
        return NoContent();
    }

    [HttpDelete("/api/tasks{taskId:int}")]
    async public Task<IActionResult> DeleteTaskAsync(int taskId)
    {
        var userId = GetCurrentPersonId();
        await _taskService.DeleteTaskAsync(userId, taskId);
        return NoContent();
    }

}