using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using QuizAPIELECT2.Models;
using QuizAPIELECT2.Utils;
using static QuizAPIELECT2.Utils.MockDataBase;

namespace QuizAPIELECT2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly jwtService _jwtService;

        public AuthController(jwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        [ApiKeyAuthorize]
        [EnableRateLimiting("LoginPolicy")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            var user = MockData.Users.FirstOrDefault(x =>
                x.Username.Equals(loginModel.Username, StringComparison.OrdinalIgnoreCase) &&
                x.Password == loginModel.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = _jwtService.GenerateToken(user.Username, user.Role);

            return Ok(new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Role = user.Role
            });
        }
    }
}