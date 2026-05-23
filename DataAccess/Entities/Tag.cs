
namespace DataAccess.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Навигационные свойства
    public User? User { get; set; }
    public ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
}

