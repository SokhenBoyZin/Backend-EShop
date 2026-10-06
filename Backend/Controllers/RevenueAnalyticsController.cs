using Backend.DTOs.Analytics;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/analytics/revenue")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class RevenueAnalyticsController : ControllerBase
    {
        private readonly IRevenueAnalyticsService _service;

        public RevenueAnalyticsController(
            IRevenueAnalyticsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetRevenue(
            [FromQuery] RevenueRequest request)
        {
            try
            {
                var result =
                    await _service.GetRevenueAsync(request);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}