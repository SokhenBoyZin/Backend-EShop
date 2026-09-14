using Backend.DTOs.Request;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectivityTypeController : ControllerBase
    {
        private readonly IConnectivityTypeService
            _connectivityTypeService;

        public ConnectivityTypeController(
            IConnectivityTypeService connectivityTypeService)
        {
            _connectivityTypeService = connectivityTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllConnectivityTypes()
        {
            var connectivityTypes =
                await _connectivityTypeService
                    .GetAllConnectivityTypes();

            return Ok(connectivityTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConnectivityTypeById(int id)
        {
            var connectivityType =
                await _connectivityTypeService
                    .GetConnectivityTypeById(id);

            if (connectivityType == null)
            {
                return NotFound(new
                {
                    message = "Connectivity type not found"
                });
            }

            return Ok(connectivityType);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateConnectivityType(
            [FromBody] ConnectivityTypeRequest? request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    message = "Request cannot be null"
                });
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new
                {
                    message = "Connectivity type name is required"
                });
            }

            var connectivityType =
                await _connectivityTypeService
                    .CreateConnectivityType(request);

            return CreatedAtAction(
                nameof(GetConnectivityTypeById),
                new { id = connectivityType.ConnectivityId },
                connectivityType
            );
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateConnectivityType(
            int id,
            [FromBody] ConnectivityTypeRequest? request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    message = "Request cannot be null"
                });
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new
                {
                    message = "Connectivity type name is required"
                });
            }

            var connectivityType =
                await _connectivityTypeService
                    .UpdateConnectivityType(id, request);

            if (connectivityType == null)
            {
                return NotFound(new
                {
                    message = "Connectivity type not found"
                });
            }

            return Ok(connectivityType);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteConnectivityType(int id)
        {
            var result =
                await _connectivityTypeService
                    .DeleteConnectivityType(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Connectivity type not found"
                });
            }

            return Ok(new
            {
                message = "Connectivity type deleted successfully"
            });
        }
    }
}