using Task_Management.Respositories.Emails;

namespace Task_Management.Services.Foundations.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailRepository emailRepository;

        public NotificationService(IEmailRepository emailRepository)
        {
            this.emailRepository = emailRepository;
        }

        public async Task SendNotificationAsync(string email, string message)
        {
            await emailRepository.SendEmailAsync(email, "Task Notification", message);
        }
    }
}
