using Task_Management.Models.Entities.UserTasks;
using Task_Management.Respositories.UserTasks;

namespace Task_Management.Services.Foundations.UserTaks
{
    public class UserTaskService : IUserTaskService
    {
        private readonly IUserTaskRepository userTaskRepository;

        public UserTaskService(IUserTaskRepository userTaskRepository)
        {
            this.userTaskRepository = userTaskRepository;
        }

        public async ValueTask<UserTask> CreateUserTaskAsync(UserTask userTask)
        {
            return await this.userTaskRepository.InsertUserTaskAsync(userTask);
        }

        public async ValueTask<List<UserTask>> RetrieveAllUserTasksAsync(Guid userId)
        {
            return await this.userTaskRepository.SelectAllUserTasksAsync(userId);
        }

        public async ValueTask<UserTask> RetrieveUserTaskByIdAsync(Guid userId, Guid taskId)
        {
            return await this.userTaskRepository.SelectUserTaskByIdAsync(userId, taskId);
        }

        public async ValueTask<UserTask> RemoveUserTaskByIdAsync(Guid userId, Guid taskId)
        {
            UserTask maybeUserTask =
                await this.userTaskRepository.SelectUserTaskByIdAsync(userId, taskId);

            return await this.userTaskRepository.DeleteUserTaskAsync(maybeUserTask);
        }
    }
}
