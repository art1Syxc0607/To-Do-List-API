using DataAccess.Entities;

namespace BusinessLogic.DTO.UserTasks;

public class UserTaskFilterDto
{
    public UserTaskStatus? Status { get; set; }
    public Priority? Priority { get; set; }
    public string? Search { get; set; }
    public string? DueDateRange { get; set; } // "today", "week", "overdue"
    public int? CategoryId { get; set; }
    public List<int>? TagIds { get; set; }
    //public int Page { get; set; } = 1;
    //public int PageSize { get; set; } = 20;
}

