using DataAccess.Entities;
using System.ComponentModel.DataAnnotations;

namespace To_Do_List_API.DTO.UserTasks;

public class UserTaskResponseDto
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public DateTime CreateTime { get; set; }
    [Required]
    public DateTime UpdateTime { get; set; }
    [Required]
    public bool IsCompleted { get; set; }
    [Required]
    public string Status { get; set; }
    [Required]
    public Priority Priority { get; set; }
}

