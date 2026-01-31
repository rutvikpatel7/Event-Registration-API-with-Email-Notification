using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EventRegistrationAPI.Services
{
    public class MailtrapEmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _mailtrapApiUrl = "https://send.api.mailtrap.io/api/send";

        public MailtrapEmailService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendConfirmationEmailAsync(string recipientEmail, string recipientName, string eventTitle, DateTime eventDate, string location, int registrationId)
        {
            var apiToken = _configuration["Mailtrap:ApiToken"];
            if (string.IsNullOrEmpty(apiToken))
            {
                throw new ArgumentException("Mailtrap API Token not configured");
            }

            var fromEmail = _configuration["Mailtrap:FromEmail"];
            var senderName = _configuration["Mailtrap:SenderName"] ?? "Event Registration System";

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

            var payload = new
            {
                from = new { email = fromEmail, name = senderName },
                to = new[] { new { email = recipientEmail, name = recipientName } },
                subject = $"Event Registration Confirmation - {eventTitle}",
                html = htmlContent
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiToken);

            var response = await _httpClient.PostAsync(_mailtrapApiUrl, content);
            response.EnsureSuccessStatusCode();
        }
    }
}