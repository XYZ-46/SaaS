using API.Authorization;
using DataEntity.Model;
using DataEntity.Pagination;
using DataEntity.Request;
using InterfaceProject.User;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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
        [AllMember]
        public IActionResult Paging(PagingRequest? pagingRequest)
        {
            BaseResponse response = new();
            pagingRequest ??= new PagingRequest();

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

        [HttpGet("getController")]
        public IActionResult GetController()
        {
            ControllerActions result = new();

            Assembly asm = Assembly.GetExecutingAssembly();
            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(MainController).IsAssignableFrom(type) && type.Name != "MainController").ToList();

            controlleractionlist.ForEach(x =>
            {
                var methods = x.GetMethods().Where(a => a.ReturnType == typeof(IActionResult)).ToList();

                result.ControllerList.Add(new ControllerInfo
                {
                    ControllerName = x.Name,
                    ActionList = methods.Select(x => x.Name).ToList()
                });

            });

            var response = new BaseResponse() { data = result };
            return Ok(response);
        }

        public record ControllerActions
        {
            public List<ControllerInfo> ControllerList { get; set; } = [];
        }

        public record ControllerInfo
        {
            public string ControllerName { get; init; }
            public List<string> ActionList { get; set; } = [];
        }
    }
}
