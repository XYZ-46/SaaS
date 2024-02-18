using DataEntity.Model;
using DataEntity.Pagination;
using DataEntity.Request;
using InterfaceProject.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;

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

            BaseResponse response = new();
            var (isValidRequest, errorList) = pagingRequest.ValidateModel<UserProfileModel>();

            if (isValidRequest)
            {
                //response = PagingRequest.GetData<UserProfileModel>();
            }
            else
            {
                response.errorProperty = errorList;
                return BadRequest(response);
            }

            return Ok(pagingRequest);
        }

    }
}
