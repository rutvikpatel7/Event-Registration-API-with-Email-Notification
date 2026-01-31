using System.Threading.Tasks;

namespace EventRegistrationAPI.Services
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string recipientEmail, string recipientName, string eventTitle, DateTime eventDate, string location, int registrationId);
    }
}