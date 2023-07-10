using Task_Management.Models.Entities.Tasks;
using Task_Management.Models.Entities.UserTasks;

namespace Task_Management.Respositories.Tasks
{
    public interface ITaskRepository
    {
        Task InsertTaskAsync(TaskEntity taskEntity);
        IQueryable<TaskEntity> SelectAllTasks();
        Task<TaskEntity> SelectTaskByIdAsync(Guid id);
        Task UpdateTaskAsync(TaskEntity taskEntity);
        Task DeleteTaskAsync(TaskEntity taskEntity);
        Task AddTaskAssignment(UserTask userTask);
        Task<List<TaskEntity>> GetUpcomingTasksAsync();
    }
}
