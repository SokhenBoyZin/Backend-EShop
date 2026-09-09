using Backend.DTOs.Request;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IColorService _colorService;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllColors()
        {
            var colors = await _colorService.GetAllColors();

            return Ok(colors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetColorById(int id)
        {
            var color = await _colorService.GetColorById(id);

            if (color == null)
                return NotFound(new
                {
                    message = "Color not found"
                });

            return Ok(color);
        }

        [HttpPost]
        public async Task<IActionResult> CreateColor([FromBody] ColorRequest request)
        {
            if (request == null) return BadRequest(new
            {
                message = "All field required!"
            });

            var color = await _colorService.CreateColor(request);

            return CreatedAtAction(
                nameof(GetColorById),
                new { id = color.ColorId },
                color
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateColor(int id, [FromBody] ColorRequest request)
        {
            var color = await _colorService.UpdateColor(id, request);

            if (color == null)
                return NotFound(new
                {
                    message = "Color not found"
                });

            return Ok(color);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColor(int id)
        {
            var result = await _colorService.DeleteColor(id);

            if (!result)
                return NotFound(new
                {
                    message = "Color not found"
                });

            return Ok(new
            {
                message = "Color deleted successfully"
            });
        }
    }
}