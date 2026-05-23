using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string UserName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }  // ← добавить для полноты

    // Навигационные свойства (НЕ nullable, а пустые коллекции)
    public ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}