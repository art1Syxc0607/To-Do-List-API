namespace BusinessLogic.DTO.Tag;

public class TagResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int TasksCount { get; set; }  // сколько задач с этим тегом
}