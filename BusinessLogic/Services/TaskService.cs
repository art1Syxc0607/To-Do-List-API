using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;


namespace BusinessLogic.Services;

public class TaskService : ITaskService
{
    private ITaskRepository _taskRepository;
    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    async public Task<List<UserTask>?> GetTasks(int userId)
    {
        return await _taskRepository.GetTasks(userId);
    }


}

