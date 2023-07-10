using Task_Management.Services.Foundations.Notifications;
using Task_Management.Services.Foundations.Tasks;
using Task_Management.Services.Orchestration.UserTasks;

namespace Task_Management.Services.Backgrounds
{
    public class NotificationReminderService : BackgroundService
    {
        private readonly ILogger<NotificationReminderService> logger;
        private readonly IServiceProvider serviceProvider;

        public NotificationReminderService(
        ILogger<NotificationReminderService> logger,
        IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                logger.LogInformation("Background Service is starting.");

                await Task.Run(async () =>
                {
                    using IServiceScope scope = serviceProvider.CreateScope();

                    IUserTaskOrchestrationService userTaskOrchestrationService =
                    scope.ServiceProvider.GetService<IUserTaskOrchestrationService>();

                    await userTaskOrchestrationService.NotifyTaskDeadlines(stoppingToken);
                });

                logger.LogInformation("Background Service stopped.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred in the Background Service.");
                throw;
            }
        }
    }
}
