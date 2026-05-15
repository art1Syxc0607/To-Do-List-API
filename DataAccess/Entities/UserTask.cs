namespace DataAccess.Entities;

public class UserTask
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsCompleted { get; set; }
    public int UserId {  get; set; }
    public User? User { get; set; }
    public Status Status { get; set; }
    public Priority Priority {  get; set; }



}

