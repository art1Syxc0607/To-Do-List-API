using DataAccess.Entities;

namespace DataAccess.Interfaces;

public interface ITaskRepository
{
    Task<List<UserTask>?> GetTasks(int userId);

}

