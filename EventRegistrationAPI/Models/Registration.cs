using System;

namespace EventRegistrationAPI.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        public Event Event { get; set; }
    }
}