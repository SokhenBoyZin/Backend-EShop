using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Request
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(50)]
        public string username { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string email { get; set; } = string.Empty;

        [Required]  
        public string password { get; set; } = string.Empty;

        [Required]
        public string confirm_password { get; set; } = string.Empty;
    }
}
