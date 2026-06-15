using System.ComponentModel.DataAnnotations;
using DataAccess.Entities;

namespace BusinessLogic.DTO.UserTasks;

public class CreateTaskDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public ICollection<int> TagsId { get; set; } = new List<int>();

    // ✅ ПРАВИЛЬНО: указываем полное имя с перечислением
    public UserTaskStatus Status { get; set; } = UserTaskStatus.NotStarted;

    // ✅ ПРАВИЛЬНО: приоритет по умолчанию
    public Priority Priority { get; set; } = Priority.Medium;
    public DateTime? DueDate { get; set; }
}

