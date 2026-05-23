namespace BusinessLogic.DTO.Tag;

public class TagResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int TasksCount { get; set; }  // сколько задач с этим тегом
}