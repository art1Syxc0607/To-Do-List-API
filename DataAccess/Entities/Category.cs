
namespace DataAccess.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Color { get; set; }  // для UI (например, "#FF5733")
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Навигационные свойства
    public User? User { get; set; }
    public ICollection<UserTask> Tasks { get; set; } = new List<UserTask>();
}

