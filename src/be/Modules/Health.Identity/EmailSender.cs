using NETCore.MailKit.Core;
using System.Threading.Tasks;

namespace Health.Identity
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly IEmailService emailService;

        public EmailSender(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            emailService.Send(email, subject, message, true);

            return Task.CompletedTask;
        }
    }
}
