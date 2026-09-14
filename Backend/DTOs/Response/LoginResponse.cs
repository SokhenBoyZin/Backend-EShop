using Backend.DTOs.User;

namespace Backend.DTOs.Response
{
    public class LoginResponse
    {
        public string message { get; set; } = string.Empty;
        public UserResponse? user { get; set; }
        public string token { get; set; } = string.Empty;
    }
}
