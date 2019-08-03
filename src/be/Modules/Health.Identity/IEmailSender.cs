using System.Threading.Tasks;

namespace Health.Identity
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
