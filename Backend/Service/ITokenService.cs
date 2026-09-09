using Backend.Models;

namespace Backend.Service
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}