using Backend.DTOs.Request;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapacityController : ControllerBase
    {
        private readonly ICapacityService _capacityService;

        public CapacityController(ICapacityService capacityService)
        {
            _capacityService = capacityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCapacities()
        {
            var capacities = await _capacityService.GetAllCapacities();

            return Ok(capacities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCapacityById(int id)
        {
            var capacity = await _capacityService.GetCapacityById(id);

            if (capacity == null)
            {
                return NotFound(new
                {
                    message = "Capacity not found"
                });
            }

            return Ok(capacity);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCapacity(
            [FromBody] CapacityRequest? request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    message = "Request cannot be null"
                });
            }

            if (string.IsNullOrWhiteSpace(request.SizeLabel))
            {
                return BadRequest(new
                {
                    message = "Capacity size is required"
                });
            }

            var capacity =
                await _capacityService.CreateCapacity(request);

            return CreatedAtAction(
                nameof(GetCapacityById),
                new { id = capacity.CapacityId },
                capacity
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCapacity(
            int id,
            [FromBody] CapacityRequest? request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    message = "Request cannot be null"
                });
            }

            if (string.IsNullOrWhiteSpace(request.SizeLabel))
            {
                return BadRequest(new
                {
                    message = "Capacity size is required"
                });
            }

            var capacity =
                await _capacityService.UpdateCapacity(id, request);

            if (capacity == null)
            {
                return NotFound(new
                {
                    message = "Capacity not found"
                });
            }

            return Ok(capacity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCapacity(int id)
        {
            var result =
                await _capacityService.DeleteCapacity(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Capacity not found"
                });
            }

            return Ok(new
            {
                message = "Capacity deleted successfully"
            });
        }
    }
}