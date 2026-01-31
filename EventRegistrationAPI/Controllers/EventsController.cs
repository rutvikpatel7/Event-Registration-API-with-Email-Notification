using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventRegistrationAPI.Services;
using EventRegistrationAPI.DTOs;

namespace EventRegistrationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventsController(EventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        public async Task<ActionResult<EventResponseDto>> CreateEvent(CreateEventDto createEventDto)
        {
            try
            {
                if (createEventDto.Date < DateTime.UtcNow)
                {
                    return BadRequest("Event date cannot be in the past");
                }

                if (createEventDto.Capacity <= 0)
                {
                    return BadRequest("Capacity must be greater than 0");
                }

                var result = await _eventService.CreateEventAsync(createEventDto);
                return CreatedAtAction(nameof(GetEventById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetAllEvents()
        {
            try
            {
                var events = await _eventService.GetAllEventsAsync();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventResponseDto>> GetEventById(int id)
        {
            try
            {
                var eventItem = await _eventService.GetEventByIdAsync(id);

                if (eventItem == null)
                {
                    return NotFound(new { message = $"Event with ID {id} not found" });
                }

                return Ok(eventItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                var result = await _eventService.DeleteEventAsync(id);

                if (!result)
                {
                    return NotFound(new { message = $"Event with ID {id} not found" });
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}