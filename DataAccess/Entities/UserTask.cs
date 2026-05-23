using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities;

public class UserTask
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public int UserId { get; set; }
    public int? CategoryId { get; set; }

    // Навигационные свойства
    public Category? Category { get; set; }
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();  // ← инициализация!
    public User? User { get; set; }
    public UserTaskStatus Status { get; set; }
    public Priority Priority { get; set; }
}

