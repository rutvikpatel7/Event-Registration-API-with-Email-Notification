using System;

namespace EventRegistrationAPI.DTOs
{
    public class RegistrationResponseDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
}