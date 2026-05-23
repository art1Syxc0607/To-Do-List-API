using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BusinessLogic.DTO.Category;
using BusinessLogic.DTO.UserTasks;

namespace To_Do_List_API.Controllers;

[ApiController]
[Route("api/categories")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponseDto>>> GetCategories()
    {
        var userId = GetCurrentUserId();
        var categories = await _categoryService.GetUserCategoriesAsync(userId);
        return Ok(categories);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] CreateCategoryDto dto)
    {
        var userId = GetCurrentUserId();
        var category = await _categoryService.CreateAsync(userId, dto);
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponseDto>> GetCategory(int id)
    {
        var userId = GetCurrentUserId();
        var category = await _categoryService.GetByIdAsync(id, userId);

        if (category == null)
            return NotFound();

        return Ok(category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
    {
        var userId = GetCurrentUserId();
        await _categoryService.UpdateAsync(userId, id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var userId = GetCurrentUserId();
        await _categoryService.DeleteAsync(userId, id);
        return NoContent();
    }

    private int GetCurrentUserId()
    {
        var claim = User.FindFirst("personId") ?? User.FindFirst(ClaimTypes.NameIdentifier);
        return int.Parse(claim!.Value);
    }
}
