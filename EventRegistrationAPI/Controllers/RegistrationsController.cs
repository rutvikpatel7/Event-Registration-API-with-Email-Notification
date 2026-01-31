using EventRegistrationAPI.DTOs;
using EventRegistrationAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventRegistrationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationsController : ControllerBase
    {
        private readonly RegistrationService _registrationService;

        public RegistrationsController(RegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("events/{eventId}/register")]
        public async Task<ActionResult<RegistrationResponseDto>> RegisterForEvent(int eventId, RegisterForEventDto registerDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(registerDto.Name))
                {
                    return BadRequest("Name is required");
                }

                if (string.IsNullOrWhiteSpace(registerDto.Email))
                {
                    return BadRequest("Email is required");
                }

                var result = await _registrationService.RegisterForEventAsync(eventId, registerDto);
                return CreatedAtAction(nameof(GetRegistration), new { registrationId = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("events/{eventId}")]
        public async Task<ActionResult<IEnumerable<RegistrationResponseDto>>> GetEventRegistrations(int eventId)
        {
            try
            {
                var registrations = await _registrationService.GetEventRegistrationsAsync(eventId);
                return Ok(registrations);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("{registrationId}", Name = "GetRegistration")]
        public async Task<ActionResult<RegistrationResponseDto>> GetRegistration(int registrationId)
        {
            try
            {
                var registration = await _registrationService.GetRegistrationByIdAsync(registrationId);

                if (registration == null)
                {
                    return NotFound(new { message = $"Registration with ID {registrationId} not found" });
                }

                return Ok(registration);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpDelete("{registrationId}")]
        public async Task<IActionResult> CancelRegistration(int registrationId)
        {
            try
            {
                var result = await _registrationService.CancelRegistrationAsync(registrationId);

                if (!result)
                {
                    return NotFound(new { message = $"Registration with ID {registrationId} not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}