using Task_Management.Models.Entities.UserTasks;

namespace Task_Management.Services.Foundations.UserTaks
{
    public interface IUserTaskService
    {
        ValueTask<UserTask> CreateUserTaskAsync(UserTask userTask);
        ValueTask<List<UserTask>> RetrieveAllUserTasksAsync(Guid userId);
        ValueTask<UserTask> RetrieveUserTaskByIdAsync(Guid userId, Guid taskId);
        ValueTask<UserTask> RemoveUserTaskByIdAsync(Guid userId, Guid taskId);
    }
}
