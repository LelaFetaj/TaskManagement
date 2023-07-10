using Task_Management.Models.Entities.UserTasks;

namespace Task_Management.Respositories.UserTasks
{
    public interface IUserTaskRepository
    {
        ValueTask<UserTask> InsertUserTaskAsync(UserTask userTask);
        ValueTask<List<UserTask>> SelectAllUserTasksAsync(Guid userId);
        ValueTask<UserTask> SelectUserTaskByIdAsync(Guid userId, Guid taskId);
        ValueTask<UserTask> DeleteUserTaskAsync(UserTask userTask);
    }
}
