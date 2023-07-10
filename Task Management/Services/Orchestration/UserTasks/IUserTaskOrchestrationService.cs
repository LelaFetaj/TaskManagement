using Task_Management.Models.DTOs.Tasks;
using Task_Management.Models.Entities.Tasks;

namespace Task_Management.Services.Orchestration.UserTasks
{
    public interface IUserTaskOrchestrationService
    {
        Task<(bool, string)> AddTaskAsync(CreateTaskRequest createTaskRequest);
        Task<List<TaskEntity>> RetrieveAllTasks();
        Task<TaskEntity> RetrieveTaskById(Guid id);
        Task<(bool, string)> UpdateTaskAsync(UpdateTaskRequest updateTaskRequest);
        Task<(bool, string)> DeleteTaskByIdAsync(Guid id);
        Task<List<UpdateTaskRequest>> GetFilteredTasksAsync(TaskFilterCriteria criteria);
        Task<(bool, string)> AssignTaskToUserAsync(Guid taskId, Guid userId);
        Task NotifyTaskDeadlines(CancellationToken stoppingToken);
        Task<(bool, string)> AddTaskCollaborators(Guid taskId, Guid userId);
    }
}
