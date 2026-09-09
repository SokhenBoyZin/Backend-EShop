using Backend.Db;
using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(ApplicationDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest dto)
        {
            if (await _context.Users.AnyAsync(u => u.email == dto.email.ToLower()))
            {
                return BadRequest("Email already exists!");
            }

            if (dto.password != dto.confirm_password)
            {
                return BadRequest("Passwords do not match!");
            }

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

            return new RegisterResponse
            {
                message = "Registration successful",
                user = new UserResponse
                {
                    id = newUser.id,
                    username = newUser.username,
                    email = newUser.email,
                    role = newUser.Role.ToString()
                },
                token = _tokenService.CreateToken(newUser)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == dto.email.ToLower());

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.password, user.passwordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            return new LoginResponse
            {
                message = "Registration successful",
                user = new UserResponse
                {
                    id = user.id,
                    username = user.username,
                    email = user.email,
                    role = user.Role.ToString()
                },
                token = _tokenService.CreateToken(user)
            };
        }
    }
}