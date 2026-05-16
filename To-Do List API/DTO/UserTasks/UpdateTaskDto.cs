using DataAccess.Entities;
using System.ComponentModel.DataAnnotations;

namespace To_Do_List_API.DTO.UserTasks;

public class UpdateTaskDto
{
    [MaxLength(200)]
    public string? Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public UserTaskStatus? Status { get; set; }
    public Priority? Priority { get; set; }
    public DateTime? DueDate { get; set; }
}
