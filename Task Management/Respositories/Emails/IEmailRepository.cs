namespace Task_Management.Respositories.Emails
{
    public interface IEmailRepository
    {
        Task SendEmailAsync(string recipientEmail, string subject, string message);
    }
}
