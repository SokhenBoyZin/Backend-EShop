using Backend.Db;
using Backend.DTOs.UserProfile;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public UserProfileService(
            ApplicationDbContext context,
            IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<UserProfileResponse?> GetMyProfileAsync(
            int userId)
        {
            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (profile == null)
                return null;

            return MapToResponse(profile);
        }

        public async Task<UserProfileResponse?> CreateProfileAsync(
            UserProfileRequest request,
            int userId)
        {
            if (request == null)
                return null;

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.id == userId);

            if (user == null)
                return null;

            var existingProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (existingProfile != null)
                return null;

            string? imageUrl = null;

            // Upload image
            if (request.Image != null)
            {
                imageUrl = await UploadImageAsync(request.Image);
            }

            var profile = new UserProfile
            {
                UserId = userId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                ProfileImage = imageUrl
            };

            _context.UserProfiles.Add(profile);

            await _context.SaveChangesAsync();

            return MapToResponse(profile);
        }

        public async Task<UserProfileResponse?> UpdateProfileAsync(
            UserProfileRequest request,
            int userId)
        {
            if (request == null)
                return null;

            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (profile == null)
                return null;

            profile.FirstName = request.FirstName;
            profile.LastName = request.LastName;
            profile.PhoneNumber = request.PhoneNumber;

            // Upload new image if provided
            if (request.Image != null)
            {
                profile.ProfileImage =
                    await UploadImageAsync(request.Image);
            }

            await _context.SaveChangesAsync();

            return MapToResponse(profile);
        }

        private async Task<string> UploadImageAsync(IFormFile image)
        {
            string uploadsFolder = Path.Combine(
                _env.WebRootPath,
                "images"
            );

            Directory.CreateDirectory(uploadsFolder);

            string fileName =
                Guid.NewGuid() +
                Path.GetExtension(image.FileName);

            string filePath = Path.Combine(
                uploadsFolder,
                fileName
            );

            using var stream = new FileStream(
                filePath,
                FileMode.Create
            );

            await image.CopyToAsync(stream);

            return $"/images/{fileName}";
        }

        private UserProfileResponse MapToResponse(
            UserProfile profile)
        {
            return new UserProfileResponse
            {
                UserProfileId = profile.UserProfileId,
                UserId = profile.UserId,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                PhoneNumber = profile.PhoneNumber,
                ProfileImage = profile.ProfileImage
            };
        }
    }
}
