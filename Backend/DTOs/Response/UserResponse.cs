using Backend.Models;

namespace Backend.DTOs.Response
{
    public class UserResponse
    {
        public int id { get; set; }
        public string username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string? role { get; set; }
    }
}
