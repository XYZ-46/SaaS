using InterfaceProject.Service;
using Microsoft.AspNetCore.Mvc;
using DataEntity.User;

namespace API.Controllers
{
    public class UserController(IUserService userService) : MainController
    {
        public readonly IUserService _userService = userService;

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
    }
}
