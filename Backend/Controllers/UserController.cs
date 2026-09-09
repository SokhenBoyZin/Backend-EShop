using Backend.Db;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/user/login")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly ApplicationDbContext _userContext;
        public UserController (ApplicationDbContext user)
        {
            _userContext = user;
        }

        
    }
}
