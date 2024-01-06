using DataEntity.User;
using InterfaceProject.Service;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace API.Controllers
{
    public class AuthController(IAuthService authService) : MainController
    {
        public readonly IAuthService _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest userLoginParamReq)
        {
            string token = await _authService.Login(userLoginParamReq);
            return Ok(new { token });
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
