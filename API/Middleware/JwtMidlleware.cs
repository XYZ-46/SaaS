using InterfaceProject.Service;
using InterfaceProject.User;

namespace API.Middleware
{
    public class JwtMidlleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context, IUserService userService, IJwtTokenService jwtService)
        {
            int? userId = null;
            var token = context.Request.Headers.Authorization.SingleOrDefault()?.Split(" ").Last();

            if (await jwtService.ValidateJwtToken(token!))
            {
                // attach user to context on successful jwt validation
                //var user = userService.GetUserById(userId.Value);

                //var menu = jwtUtils.GetUserMenuAccess(user);
                //var menuAction = userService.GetUserMenuAction(menu);

                //context.Items["User"] = user;
            }

            await this._next(context);
        }
    }
}
