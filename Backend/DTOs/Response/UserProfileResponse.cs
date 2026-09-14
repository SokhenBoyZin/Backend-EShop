namespace Backend.DTOs.UserProfile
{
    public class UserProfileResponse
    {
        public int UserProfileId { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? ProfileImage { get; set; }
    }
}