using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.UserProfile
{
    public class UserProfileRequest
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public IFormFile? Image { get; set; }
    }
}