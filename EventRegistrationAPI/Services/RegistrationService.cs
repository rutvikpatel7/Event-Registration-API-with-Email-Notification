using EventRegistrationAPI.Data;
using EventRegistrationAPI.DTOs;
using EventRegistrationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventRegistrationAPI.Services
{
    public class RegistrationService
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public RegistrationService(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<RegistrationResponseDto> RegisterForEventAsync(int eventId, RegisterForEventDto registerDto)
        {
            var eventEntity = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventEntity == null)
            {
                throw new ArgumentException($"Event with ID {eventId} not found");
            }

            // Business Rule: Registration for past events is not allowed
            if (eventEntity.Date < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Cannot register for past events");
            }

            // Business Rule: Event capacity cannot be exceeded
            if (eventEntity.Registrations.Count >= eventEntity.Capacity)
            {
                throw new InvalidOperationException("Event is at full capacity");
            }

            // Business Rule: Same email cannot register twice for the same event
            if (eventEntity.Registrations.Any(r => r.Email == registerDto.Email))
            {
                throw new InvalidOperationException("This email is already registered for this event");
            }

            var registration = new Registration
            {
                EventId = eventId,
                Name = registerDto.Name,
                Email = registerDto.Email
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            // Send confirmation email
            await _emailService.SendConfirmationEmailAsync(
                registration.Email,
                registration.Name,
                eventEntity.Title,
                eventEntity.Date,
                eventEntity.Location,
                registration.Id
            );

            return MapToRegistrationResponseDto(registration);
        }

        public async Task<RegistrationResponseDto> GetRegistrationByIdAsync(int registrationId)
        {
            var registration = await _context.Registrations.FindAsync(registrationId);

            if (registration == null)
            {
                return null;
            }

            return MapToRegistrationResponseDto(registration);
        }

        public async Task<IEnumerable<RegistrationResponseDto>> GetEventRegistrationsAsync(int eventId)
        {
            var eventEntity = await _context.Events.FindAsync(eventId);

            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Event with ID {eventId} not found");
            }

            var registrations = await _context.Registrations
                .Where(r => r.EventId == eventId)
                .ToListAsync();

            return registrations.Select(MapToRegistrationResponseDto);
        }

        public async Task<IEnumerable<RegistrationResponseDto>> GetRegistrationsByEventAsync(int eventId)
        {
            var registrations = await _context.Registrations
                .Where(r => r.EventId == eventId)
                .ToListAsync();

            return registrations.Select(MapToRegistrationResponseDto);
        }

        public async Task<bool> CancelRegistrationAsync(int registrationId)
        {
            var registration = await _context.Registrations.FindAsync(registrationId);

            if (registration == null)
            {
                return false;
            }

            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();

            return true;
        }

        private RegistrationResponseDto MapToRegistrationResponseDto(Registration registration)
        {
            return new RegistrationResponseDto
            {
                Id = registration.Id,
                EventId = registration.EventId,
                Name = registration.Name,
                Email = registration.Email,
                RegisteredAt = registration.RegisteredAt
            };
        }
    }
}