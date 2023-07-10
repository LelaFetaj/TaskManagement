using SendGrid.Helpers.Mail;
using SendGrid;
using System.Net.Mail;
using System.Net;

namespace Task_Management.Respositories.Emails
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration configuration;
        private readonly SmtpClient smtpClient;

        public EmailRepository(IConfiguration configuration)
        {
            this.configuration = configuration;

            this.smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = false,
                Credentials = new NetworkCredential(
                    configuration["EmailConfiguration:Email"],
                    configuration["EmailConfiguration:Password"])
            };
        }

        public async Task SendEmailAsync(string recipientEmail, string subject, string message)
        {

            var mailMessage = new MailMessage(
                from: configuration["EmailConfiguration:Email"],
                to: recipientEmail,
                subject,
                body: message);

            await this.smtpClient.SendMailAsync(mailMessage);
        }

    }
}
