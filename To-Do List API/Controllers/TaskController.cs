//using BusinessLogic.Services;
using BusinessLogic.DTO.UserTasks;
using BusinessLogic.DTO.Tag;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        var tasks = await _taskService.GetTasksAsync(userId);
        return Ok(tasks);
    }

    [HttpGet("/api/tasks{taskId:int}")]
    async public Task<ActionResult<UserTaskResponseDto>> GetTask(int taskId)
    {
        var userId = GetCurrentPersonId();
        var task = await _taskService.GetTaskAsync(userId, taskId);

        if (task == null)
            return NotFound();  // ← 404, а не 200

        return Ok(task);
    }

    [HttpPost("/api/tasks")]
    async public Task<IActionResult> CreateTaskAsync([FromBody] CreateTaskDto createdto)
    {
        var userId = GetCurrentPersonId();
        await _taskService.CreateTaskAsync(userId, createdto.CategoryId, createdto.TagsId, createdto.Description, createdto.Title, createdto.Status, 
            createdto.Priority, createdto.DueDate);
        return NoContent();
    }

    [HttpPut("/api/tasks/{taskId:int}")]
    async public Task<IActionResult> UpdateTaskAsync(int taskId, [FromBody] UpdateTaskDto updatedto)
    {
        var userId = GetCurrentPersonId();
        await _taskService.UpdateTaskAsync(userId, taskId, updatedto.Description, updatedto.Title,
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


    // ========== УПРАВЛЕНИЕ КАТЕГОРИЕЙ ==========

    // PUT /api/tasks/5/category/10
    [HttpPut("{taskId}/category/{categoryId}")]
    public async Task<IActionResult> SetCategory(int taskId, int categoryId)
    {
        var userId = GetCurrentPersonId();
        await _taskService.SetCategoryAsync(userId, taskId, categoryId);
        return NoContent();
    }

    // DELETE /api/tasks/5/category
    [HttpDelete("{taskId}/category")]
    public async Task<IActionResult> RemoveCategory(int taskId)
    {
        var userId = GetCurrentPersonId();
        await _taskService.RemoveCategoryAsync(userId, taskId);
        return NoContent();
    }


    // ========== УПРАВЛЕНИЕ ТЕГАМИ ==========

    // POST /api/tasks/5/tags/10
    [HttpPost("{taskId}/tags/{tagId}")]
    public async Task<IActionResult> AddTag(int taskId, int tagId)
    {
        var userId = GetCurrentPersonId();
        await _taskService.AddTagAsync(userId, taskId, tagId);
        return NoContent();
    }

    // DELETE /api/tasks/5/tags/10
    [HttpDelete("{taskId}/tags/{tagId}")]
    public async Task<IActionResult> RemoveTag(int taskId, int tagId)
    {
        var userId = GetCurrentPersonId();
        await _taskService.RemoveTagAsync(userId, taskId, tagId);
        return NoContent();
    }

    // GET /api/tasks/5/tags
    [HttpGet("{taskId}/tags")]
    public async Task<ActionResult<List<TagResponseDto>>> GetTags(int taskId)
    {
        var userId = GetCurrentPersonId();
        var tags = await _taskService.GetTagsAsync(userId, taskId);
        return Ok(tags);
    }

    // PUT /api/tasks/5/tags
    [HttpPut("{taskId}/tags")]
    public async Task<IActionResult> SetTags(int taskId, [FromBody] List<int> tagIds)
    {
        var userId = GetCurrentPersonId();
        await _taskService.SetTagsAsync(userId, taskId, tagIds);
        return NoContent();
    }

    // ========== Фильтрация и Поиск ==========
    [HttpGet("/api/tasks/filter")]
    public async Task<IActionResult> GetTasksFilter([FromQuery] UserTaskFilterDto taskfilterdto)
    {
        int userId = GetCurrentPersonId();
        var tasks = await _taskService.GetFilteredTasksAsync(userId, taskfilterdto);
        return Ok(tasks);
    }

}