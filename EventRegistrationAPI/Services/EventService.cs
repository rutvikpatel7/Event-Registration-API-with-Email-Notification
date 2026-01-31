using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventRegistrationAPI.Data;
using EventRegistrationAPI.Models;
using EventRegistrationAPI.DTOs;

namespace EventRegistrationAPI.Services
{
    public class EventService
    {
        private readonly AppDbContext _context;

        public EventService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EventResponseDto> CreateEventAsync(CreateEventDto createEventDto)
        {
            var eventEntity = new Event
            {
                Title = createEventDto.Title,
                Description = createEventDto.Description,
                Date = createEventDto.Date,
                Capacity = createEventDto.Capacity,
                Location = createEventDto.Location
            };

            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync();

            return MapToEventResponseDto(eventEntity);
        }

        public async Task<IEnumerable<EventResponseDto>> GetAllEventsAsync()
        {
            var events = await _context.Events
                .Include(e => e.Registrations)
                .ToListAsync();

            return events.Select(MapToEventResponseDto);
        }

        public async Task<EventResponseDto> GetEventByIdAsync(int eventId)
        {
            var eventEntity = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventEntity == null)
            {
                return null;
            }

            return MapToEventResponseDto(eventEntity);
        }

        public async Task<bool> DeleteEventAsync(int eventId)
        {
            var eventEntity = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventEntity == null)
            {
                return false;
            }

            if (eventEntity.Registrations.Any())
            {
                throw new InvalidOperationException("Cannot delete event with existing registrations");
            }

            _context.Events.Remove(eventEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        private EventResponseDto MapToEventResponseDto(Event eventEntity)
        {
            return new EventResponseDto
            {
                Id = eventEntity.Id,
                Title = eventEntity.Title,
                Description = eventEntity.Description,
                Date = eventEntity.Date,
                Capacity = eventEntity.Capacity,
                RegisteredCount = eventEntity.Registrations?.Count ?? 0,
                Location = eventEntity.Location,
                CreatedAt = eventEntity.CreatedAt
            };
        }
    }
}