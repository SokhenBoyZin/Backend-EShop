using Backend.DTOs.User;

namespace Backend.Services
{
    public interface IUserService
    {
        Task<UserResponse?> GetMyProfileAsync(int userId);

        Task<UserResponse?> UpdateMyProfileAsync(
            int userId,
            UserRequest request);

        Task<List<UserResponse>> GetAllUsersAsync();

        Task<UserResponse?> GetUserByIdAsync(
            int userId);

        Task<UserResponse?> UpdateUserAsync(
            int userId,
            UserRequest request);

        Task<bool> UpdateRoleAsync(
            int userId,
            UpdateRoleRequest request);

        Task<bool> DeleteUserAsync(
            int userId);
    }
}