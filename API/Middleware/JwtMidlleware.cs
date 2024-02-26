using InterfaceProject.Service;
using InterfaceProject.User;

namespace API.Middleware
{
    public class JwtMidlleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context, IUserService userService, IJwtTokenService jwtService)
        {
            var token = context.Request.Headers.Authorization.SingleOrDefault()?.Split(" ").LastOrDefault() ?? throw new ArgumentException("Invalid Token");

            var (isValidToken, userID) = jwtService.ValidateJwtToken(token!);

            if (isValidToken)
            {
                // attach user to context on successful jwt validation
                var user = await userService.FindUserByID(userID) ?? throw new ArgumentException("Can not find user");
                context.Items["User"] = user;
            }

            await this._next(context);
        }
    }
}
