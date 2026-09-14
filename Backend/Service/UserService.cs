using Backend.Db;
using Backend.DTOs.User;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET MY PROFILE
        public async Task<UserResponse?> GetMyProfileAsync(
            int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.id == userId);

            if (user == null)
            {
                return null;
            }

            return MapToResponse(user);
        }


        // UPDATE MY PROFILE
        public async Task<UserResponse?> UpdateMyProfileAsync(
            int userId,
            UserRequest request)
        {
            if (request == null)
            {
                return null;
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.id == userId);

            if (user == null)
            {
                return null;
            }

            // Check duplicate username
            var usernameExists =
                await _context.Users.AnyAsync(x =>
                    x.id != userId &&
                    x.username == request.Username);

            if (usernameExists)
            {
                return null;
            }

            // Check duplicate email
            var emailExists =
                await _context.Users.AnyAsync(x =>
                    x.id != userId &&
                    x.email == request.Email);

            if (emailExists)
            {
                return null;
            }

            user.username = request.Username;
            user.email = request.Email;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToResponse(user);
        }

        // GET ALL USERS
        public async Task<List<UserResponse>>
            GetAllUsersAsync()
        {
            var users = await _context.Users
                .OrderByDescending(x => x.id)
                .ToListAsync();

            return users
                .Select(MapToResponse)
                .ToList();
        }


        // GET USER BY ID
        public async Task<UserResponse?>
            GetUserByIdAsync(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.id == userId);

            if (user == null)
            {
                return null;
            }

            return MapToResponse(user);
        }


        // ADMIN UPDATE USER
        public async Task<UserResponse?>
            UpdateUserAsync(
                int userId,
                UserRequest request)
        {
            if (request == null)
            {
                return null;
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.id == userId);

            if (user == null)
            {
                return null;
            }

            var usernameExists =
                await _context.Users.AnyAsync(x =>
                    x.id != userId &&
                    x.username == request.Username);

            if (usernameExists)
            {
                return null;
            }

            var emailExists =
                await _context.Users.AnyAsync(x =>
                    x.id != userId &&
                    x.email == request.Email);

            if (emailExists)
            {
                return null;
            }

            user.username = request.Username;
            user.email = request.Email;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToResponse(user);
        }


        // ADMIN UPDATE ROLE
        public async Task<bool>
            UpdateRoleAsync(
                int userId,
                UpdateRoleRequest request)
        {
            if (request == null)
            {
                return false;
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.id == userId);

            if (user == null)
            {
                return false;
            }

            user.Role = request.Role;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }


        // ADMIN DELETE USER
        public async Task<bool>
            DeleteUserAsync(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.id == userId);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return true;
        }

        private UserResponse MapToResponse(
            User user)
        {
            return new UserResponse
            {
                Id = user.id,

                Username =
                    user.username,

                Email =
                    user.email,

                Role =
                    user.Role,

                CreatedAt =
                    user.CreatedAt,

                UpdatedAt =
                    user.UpdatedAt
            };
        }
    }
}