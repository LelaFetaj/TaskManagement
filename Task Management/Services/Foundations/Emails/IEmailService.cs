namespace Task_Management.Services.Foundations.Emails
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipientEmail, string subject, string message);
    }
}
