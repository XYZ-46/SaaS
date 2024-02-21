using DataEntity.Model;
using DataEntity.Pagination;
using DataEntity.Request;
using InterfaceProject.User;
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

        [HttpGet("page")]
        public IActionResult Paging(PagingRequest pagingRequest)
        {

            BaseResponse response = new();
            var (isValidRequest, errorList) = pagingRequest.ValidateModel<UserProfileModel>();

            if (isValidRequest)
            {
                response.page = _userService.GetPagingData(pagingRequest);
            }
            else
            {
                response.errorProperty = errorList;
                return BadRequest(response);
            }

            return Ok(response);
        }

    }
}
