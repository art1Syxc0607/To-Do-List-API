using BusinessLogic.Services;
using DataAccess.Entities;

namespace BusinessLogic.Interfaces;

public interface ITaskService
{
    Task<List<UserTask>?> GetTasks(int userId);

}

