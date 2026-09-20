using Backend.Db;
using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.DTOs.User;
using Backend.Models;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(
            ApplicationDbContext context,
            ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register(
            [FromBody] RegisterRequest dto)
        {
            // Check email
            if (await _context.Users.AnyAsync(
                u => u.email == dto.email.ToLower()))
            {
                return BadRequest(new
                {
                    message = "Email already exists!"
                });
            }

            // Check password confirmation
            if (dto.password != dto.confirm_password)
            {
                return BadRequest(new
                {
                    message = "Passwords do not match!"
                });
            }

            // Create user
            var newUser = new User
            {
                username = dto.username,
                email = dto.email.ToLower(),
                passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.password),
                Role = Role.USER,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Return response
            return Ok(new RegisterResponse
            {
                message = "Registration successful",

                user = new UserResponse
                {
                    Id = newUser.id,
                    Username = newUser.username,
                    Email = newUser.email,
                    Role = newUser.Role,
                    CreatedAt = newUser.CreatedAt,
                    UpdatedAt = newUser.UpdatedAt
                },

                token = _tokenService.CreateToken(newUser)
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(
            [FromBody] LoginRequest dto)
        {
            var email = dto.email.ToLower();

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.email == email);

            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(
                    dto.password,
                    user.passwordHash))
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            // Return response
            return Ok(new LoginResponse
            {
                message = "Login successful",

                user = new UserResponse
                {
                    Id = user.id,
                    Username = user.username,
                    Email = user.email,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                },

                token = _tokenService.CreateToken(user)
            });
        }
    }
}