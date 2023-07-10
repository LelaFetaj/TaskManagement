using Task_Management.Models.DTOs.Tasks;
using Task_Management.Models.Entities.Tasks;

namespace Task_Management.Services.Foundations.Tasks
{
    public interface ITaskService
    {
        Task<(bool, string)> AddTaskAsync(CreateTaskRequest createTaskRequest);

        Task<List<TaskEntity>> RetrieveAllTasks();

        Task<TaskEntity> RetrieveTaskById(Guid id);

        Task<(bool, string)> UpdateTaskAsync(UpdateTaskRequest updateTaskRequest);

        Task<(bool, string)> DeleteTaskByIdAsync(Guid id);

        Task<List<UpdateTaskRequest>> GetFilteredTasksAsync(TaskFilterCriteria criteria);

        Task<List<UpdateTaskRequest>> GetUpcomingTasksAsync();
    }
}
