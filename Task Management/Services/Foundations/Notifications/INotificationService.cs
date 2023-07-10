namespace Task_Management.Services.Foundations.Notifications
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string email, string message);
    }
}
