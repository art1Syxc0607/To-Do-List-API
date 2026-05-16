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

    // ✅ ПРАВИЛЬНО: указываем полное имя с перечислением
    public UserTaskStatus Status { get; set; } = UserTaskStatus.NotStarted;

    // ✅ ПРАВИЛЬНО: приоритет по умолчанию
    public Priority Priority { get; set; } = Priority.Medium;
    public DateTime? DueDate { get; set; }
}

