using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AuthController : MainController
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            return Ok();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword()
        {
            return Ok();
        }
    }
}
