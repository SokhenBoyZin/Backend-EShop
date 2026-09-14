using Backend.DTOs.UserProfile;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/user-profile")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _service;

        public UserProfileController(
            IUserProfileService service)
        {
            _service = service;
        }

        // GET: api/user-profile
        [HttpGet]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var profile = await _service.GetMyProfileAsync(
                userId.Value);

            if (profile == null)
            {
                return NotFound(new
                {
                    message = "User profile not found."
                });
            }

            return Ok(profile);
        }

        // POST: api/user-profile
        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromForm] UserProfileRequest request)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var profile = await _service.CreateProfileAsync(
                request,
                userId.Value);

            if (profile == null)
            {
                return BadRequest(new
                {
                    message = "Unable to create user profile."
                });
            }

            return Ok(profile);
        }
        // PUT: api/user-profile
        [HttpPut]
        public async Task<IActionResult> UpdateProfile(
            [FromForm] UserProfileRequest request)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var profile = await _service.UpdateProfileAsync(
                request,
                userId.Value);

            if (profile == null)
            {
                return NotFound(new
                {
                    message = "User profile not found."
                });
            }

            return Ok(profile);
        }

        private int? GetUserId()
        {
            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return null;

            if (!int.TryParse(
                userIdClaim.Value,
                out int userId))
                return null;

            return userId;
        }
    }
}