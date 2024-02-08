using API.Pagination;
using DataEntity.Request;
using InterfaceProject.Service;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserController(IUserService userService) : MainController
    {
        public readonly IUserService _userService = userService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest userRegisterParamReq)
        {
            await _userService.Register(userRegisterParamReq);
            return Ok();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging(PagingRequest pagingRequest)
        {

            return Ok();
        }
     
    }
}
