using Backend.DTOs.DeliveryLocation;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/delivery-locations")]
    [Authorize]
    public class DeliveryLocationController : ControllerBase
    {
        private readonly IDeliveryLocationService _service;

        public DeliveryLocationController(
            IDeliveryLocationService service)
        {
            _service = service;
        }


        // GET: api/delivery-locations
        [HttpGet]
        public async Task<IActionResult> GetMyLocations()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var locations =
                await _service.GetMyLocationsAsync(userId.Value);

            return Ok(locations);
        }


        // GET: api/delivery-locations/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var location =
                await _service.GetByIdAsync(id, userId.Value);

            if (location == null)
                return NotFound(new
                {
                    message = "Delivery location not found."
                });

            return Ok(location);
        }


        // POST: api/delivery-locations
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] DeliveryLocationRequest request)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var location =
                await _service.CreateAsync(
                    request,
                    userId.Value);

            if (location == null)
            {
                return BadRequest(new
                {
                    message = "Invalid delivery location data."
                });
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = location.DeliveryLocationId },
                location);
        }


        // PUT: api/delivery-locations/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] DeliveryLocationRequest request)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var location =
                await _service.UpdateAsync(
                    id,
                    request,
                    userId.Value);

            if (location == null)
            {
                return NotFound(new
                {
                    message = "Delivery location not found."
                });
            }

            return Ok(location);
        }


        // DELETE: api/delivery-locations/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var deleted =
                await _service.DeleteAsync(
                    id,
                    userId.Value);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Delivery location not found."
                });
            }

            return Ok(new
            {
                message = "Delivery location deleted successfully."
            });
        }


        // Get current logged-in user's ID
        private int? GetUserId()
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out int userId))
                return userId;

            return null;
        }
    }
}