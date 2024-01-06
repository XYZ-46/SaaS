using InterfaceProject.Service;
using Microsoft.AspNetCore.Mvc;
using DataEntity.User;
using InterfaceProject.Search;
using Service;

namespace API.Controllers
{
    public class UserController(IUserService userService, IAuthService authService) : MainController
    {
        public readonly IUserService _userService = userService;
        public readonly IAuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest userRegisterParamReq)
        {
            try
            {
                await _userService.Register(userRegisterParamReq);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest userLoginParamReq)
        {
            try
            {
                string token = await _authService.Login(userLoginParamReq);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
