using Backend.DTOs.UserProfile;

namespace Backend.Services
{
    public interface IUserProfileService
    {
        Task<UserProfileResponse?> GetMyProfileAsync(int userId);

        Task<UserProfileResponse?> CreateProfileAsync(
            UserProfileRequest request,
            int userId);

        Task<UserProfileResponse?> UpdateProfileAsync(
            UserProfileRequest request,
            int userId);
    }
}