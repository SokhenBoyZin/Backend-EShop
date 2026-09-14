using Backend.DTOs.User;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        // GET: api/users/me
        // Customer gets their own profile
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var user =
                await _service.GetMyProfileAsync(
                    userId.Value);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(user);
        }


        // PUT: api/users/me
        // Customer updates their own profile
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile(
            [FromBody] UserRequest request)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var user =
                await _service.UpdateMyProfileAsync(
                    userId.Value,
                    request);

            if (user == null)
            {
                return BadRequest(new
                {
                    message =
                        "Unable to update profile. Username or email may already exist."
                });
            }

            return Ok(user);
        }

        // GET: api/users/admin
        // Admin gets all users
        [HttpGet("admin")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users =
                await _service.GetAllUsersAsync();

            return Ok(users);
        }


        // GET: api/users/admin/{id}
        // Admin gets any user
        [HttpGet("admin/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetUserById(
            int id)
        {
            var user =
                await _service.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(user);
        }


        // PUT: api/users/admin/{id}
        // Admin updates user
        [HttpPut("admin/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateUser(
            int id,
            [FromBody] UserRequest request)
        {
            var user =
                await _service.UpdateUserAsync(
                    id,
                    request);

            if (user == null)
            {
                return BadRequest(new
                {
                    message =
                        "Unable to update user."
                });
            }

            return Ok(user);
        }


        // PUT: api/users/admin/{id}/role
        // Admin changes user role
        [HttpPut("admin/{id}/role")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateRole(
            int id,
            [FromBody] UpdateRoleRequest request)
        {
            var result =
                await _service.UpdateRoleAsync(
                    id,
                    request);

            if (!result)
            {
                return BadRequest(new
                {
                    message =
                        "Unable to update user role."
                });
            }

            return Ok(new
            {
                message =
                    "User role updated successfully."
            });
        }


        // DELETE: api/users/admin/{id}
        // Admin deletes user
        [HttpDelete("admin/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteUser(
            int id)
        {
            var result =
                await _service.DeleteUserAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(new
            {
                message =
                    "User deleted successfully."
            });
        }

        // Get user info
        private int? GetUserId()
        {
            var userIdClaim =
                User.FindFirst(
                    ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return null;
            }

            if (!int.TryParse(
                userIdClaim.Value,
                out int userId))
            {
                return null;
            }

            return userId;
        }
    }
}