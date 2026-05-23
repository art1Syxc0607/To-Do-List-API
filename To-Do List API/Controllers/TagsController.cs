using BusinessLogic.DTO.Tag;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace To_Do_List_API.Controllers;

[ApiController]
[Route("api/tags")]
[Authorize]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }

    private int GetCurrentUserId()
    {
        var claim = User.FindFirst("personId") ?? User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
            throw new UnauthorizedAccessException("Пользователь не аутентифицирован");
        return int.Parse(claim.Value);
    }

    [HttpGet]
    public async Task<ActionResult<List<TagResponseDto>>> GetTags()
    {
        var userId = GetCurrentUserId();
        var tags = await _tagService.GetUserTagsAsync(userId);
        return Ok(tags);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagResponseDto>> GetTag(int id)
    {
        var userId = GetCurrentUserId();
        var tag = await _tagService.GetByIdAsync(userId, id);

        if (tag == null)
            return NotFound();

        return Ok(tag);
    }

    [HttpPost]
    public async Task<ActionResult<TagResponseDto>> CreateTag([FromBody] CreateTagDto dto)
    {
        var userId = GetCurrentUserId();
        var tag = await _tagService.CreateAsync(userId, dto);
        return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, tag);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTag(int id, [FromBody] UpdateTagDto dto)
    {
        var userId = GetCurrentUserId();
        await _tagService.UpdateAsync(userId, id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(int id)
    {
        var userId = GetCurrentUserId();
        await _tagService.DeleteAsync(userId, id);
        return NoContent();
    }
}