using Task_Management.Models.DTOs.Tasks;
using Task_Management.Models.Entities.Tasks;
using Task_Management.Models.Entities.Users;
using Task_Management.Models.Entities.UserTasks;
using Task_Management.Services.Backgrounds;
using Task_Management.Services.Foundations.Notifications;
using Task_Management.Services.Foundations.Tasks;
using Task_Management.Services.Foundations.Users;
using Task_Management.Services.Foundations.UserTaks;

namespace Task_Management.Services.Orchestration.UserTasks
{
    public class UserTaskOrchestrationService : IUserTaskOrchestrationService
    {
        private readonly ITaskService taskService;
        private readonly IUserTaskService userTaskService;
        private readonly INotificationService notificationService;
        private readonly IUserService userService;
        private readonly ILogger<NotificationReminderService> logger;

        public UserTaskOrchestrationService(ITaskService taskService,
            IUserTaskService userTaskService,
            INotificationService notificationService,
            IUserService userService,
            ILogger<NotificationReminderService> logger)
        {

            this.taskService = taskService;
            this.userTaskService = userTaskService;
            this.notificationService = notificationService;
            this.userService = userService;
            this.logger = logger;
        }

        public async Task<(bool, string)> AddTaskAsync(CreateTaskRequest createTaskRequest) =>
            await this.taskService.AddTaskAsync(createTaskRequest);

        public async Task<(bool, string)> AssignTaskToUserAsync(Guid taskId, Guid userId)
        {
            TaskEntity task = await taskService.RetrieveTaskById(taskId);
            User user = await userService.RetreiveUserByIdAsync(userId);

            if (task == null || user == null)
            {
                return (false, "Task or User not found");
            }

            if (task.AssignedUserId == userId)
            {
                return (false, "The task is already assigned to the specified user");
            }

            // Assign the task to the user
            task.AssignedUserId = userId;
            await taskService.UpdateTaskAsync(new UpdateTaskRequest
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                Urgency = task.Urgency,
                DueDate = task.DueDate
            });

            // Notify the user about the task assignment
            if (user != null)
            {
                string notificationMessage = $"Task assigned to {user.UserName}";
                await notificationService.SendNotificationAsync(user.UserName, notificationMessage);
            }

            return (true, "Task assigned to the user successfully");
        }

        public async Task<(bool, string)> AddTaskCollaborators(Guid taskId, Guid userId)
        {
            TaskEntity task = await taskService.RetrieveTaskById(taskId);
            User user = await userService.RetreiveUserByIdAsync(userId);

            if (task == null || user == null)
            {
                return (false, "Task or User not found");
            }

            if (task.AssignedUserId == userId)
            {
                return (false, "The task is already assigned to the specified user");
            }

            // Create a user task entry for tracking
            UserTask userTask = new UserTask
            {
                UserId = userId,
                TaskId = taskId,
            };

            await userTaskService.CreateUserTaskAsync(userTask);

            // Notify the user about the task assignment
            if (user != null)
            {
                string notificationMessage = $"{user.UserName} have been added as a collaborator to task {task.Title}";
                await notificationService.SendNotificationAsync(user.UserName, notificationMessage);
            }
            return (true, "Collaborator added to the task.");
        }

        public async Task<(bool, string)> DeleteTaskByIdAsync(Guid id) =>
            await this.taskService.DeleteTaskByIdAsync(id);

        public async Task<List<UpdateTaskRequest>> GetFilteredTasksAsync(TaskFilterCriteria criteria) =>
            await this.taskService.GetFilteredTasksAsync(criteria);

        public async Task NotifyTaskDeadlines(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    logger.LogInformation("Checking task deadlines...");

                    // Get the upcoming tasks
                    var upcomingTasks = await taskService.GetUpcomingTasksAsync();

                    foreach (var task in upcomingTasks)
                    {
                        User user = await userService.RetreiveUserByIdAsync(task.AssignedUserId ?? Guid.Empty);
                        if (user != null)
                        {
                            var message = $"Reminder: Task '{task.Title}' is due on {task.DueDate?.ToString("d")}.";
                            await notificationService.SendNotificationAsync(user.UserName, message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error checking task deadlines.");
                }

                // Wait for 24 hours before checking again
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }

            logger.LogInformation("Task Deadline Notification Service stopped.");
        }

        public async Task<List<TaskEntity>> RetrieveAllTasks() =>
            await this.taskService.RetrieveAllTasks();

        public async Task<TaskEntity> RetrieveTaskById(Guid id) =>
            await this.taskService.RetrieveTaskById(id);

        public async Task<(bool, string)> UpdateTaskAsync(UpdateTaskRequest updateTaskRequest) =>
            await this.taskService.UpdateTaskAsync(updateTaskRequest);
    }
}
