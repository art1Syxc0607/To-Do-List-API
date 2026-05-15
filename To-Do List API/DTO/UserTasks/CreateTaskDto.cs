using System.ComponentModel.DataAnnotations;
using DataAccess.Entities;

namespace To_Do_List_API.DTO.UserTasks;

public class CreateTaskDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    //public Status? Status { get; set; } = Status.Medium;
    //public TaskPriority? Priority { get; set; } = TaskPriority.Medium;
    //public DateTime? DueDate { get; set; }
}

