using DataAccess.Entities;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.UserTasks;

public class UserTaskResponseDto
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public int? CategoryId { get; set; }
    [Required]
    public DateTime CreateTime { get; set; }
    [Required]
    public DateTime UpdateTime { get; set; }
    [Required]
    public bool IsCompleted { get; set; }
    [Required]
    public UserTaskStatus Status { get; set; }
    [Required]
    public Priority Priority { get; set; }
    public ICollection<Tag.TagResponseDto> Tags { get; set; } = new List<Tag.TagResponseDto>();
}

