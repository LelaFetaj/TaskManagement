using Task_Management.Respositories.Emails;

namespace Task_Management.Services.Foundations.Emails
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository emailRepository;

        public EmailService(IEmailRepository emailRepository)
        {
            this.emailRepository = emailRepository;
        }

        public async Task SendEmailAsync(string recipientEmail, string subject, string message)
        {
            await emailRepository.SendEmailAsync(recipientEmail, subject, message);
        }
    }
}
