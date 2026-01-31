using System;
using System.Threading.Tasks;

namespace EventRegistrationAPI.Services
{
    public class ConsoleEmailService : IEmailService
    {
        public async Task SendConfirmationEmailAsync(string recipientEmail, string recipientName, string eventTitle, DateTime eventDate, string location, int registrationId)
        {
            await Task.Run(() =>
            {
                Console.WriteLine("=== EMAIL CONFIRMATION ===");
                Console.WriteLine($"To: {recipientEmail}");
                Console.WriteLine($"Name: {recipientName}");
                Console.WriteLine($"Event: {eventTitle}");
                Console.WriteLine($"Date: {eventDate:yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine($"Location: {location}");
                Console.WriteLine($"Registration ID: {registrationId}");
                Console.WriteLine("===========================\n");
            });
        }
    }
}