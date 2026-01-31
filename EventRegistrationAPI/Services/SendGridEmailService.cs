using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;

namespace EventRegistrationAPI.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly SendGridClient _sendGridClient;
        private readonly IConfiguration _configuration;

        public SendGridEmailService(IConfiguration configuration)
        {
            var apiKey = configuration["SendGrid:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException("SendGrid API Key not configured");
            }

            _sendGridClient = new SendGridClient(apiKey);
            _configuration = configuration;
        }

        public async Task SendConfirmationEmailAsync(string recipientEmail, string recipientName, string eventTitle, DateTime eventDate, string location, int registrationId)
        {
            var fromEmail = _configuration["SendGrid:FromEmail"];
            var senderName = _configuration["SendGrid:SenderName"] ?? "Event Registration System";

            var from = new EmailAddress(fromEmail, senderName);
            var to = new EmailAddress(recipientEmail, recipientName);

            var subject = $"Event Registration Confirmation - {eventTitle}";
            var htmlContent = $@"
                <h2>Event Registration Confirmation</h2>
                <p>Dear {recipientName},</p>
                <p>Thank you for registering for our event!</p>
                <h3>Event Details:</h3>
                <ul>
                    <li><strong>Event Name:</strong> {eventTitle}</li>
                    <li><strong>Date:</strong> {eventDate:yyyy-MM-dd HH:mm:ss}</li>
                    <li><strong>Location:</strong> {location}</li>
                    <li><strong>Registration ID:</strong> {registrationId}</li>
                </ul>
                <p>We look forward to seeing you at the event!</p>
                <p>Best regards,<br/>Event Registration Team</p>
            ";

            var msg = new SendGridMessage()
            {
                From = from,
                Subject = subject,
                HtmlContent = htmlContent
            };

            msg.AddTo(to);

            await _sendGridClient.SendEmailAsync(msg);
        }
    }
}