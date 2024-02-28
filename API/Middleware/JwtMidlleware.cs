using InterfaceProject.Service;
using InterfaceProject.User;

namespace API.Middleware
{
    public class JwtMidlleware(IUserService userService, IJwtTokenService jwtService) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Headers.Authorization.SingleOrDefault()?.Split(" ").LastOrDefault();

            if (!string.IsNullOrWhiteSpace(token))
            {
                var (isValidToken, userID) = jwtService.ValidateJwtToken(token!);

                if (isValidToken)
                {
                    // attach user to context on successful jwt validation
                    var user = await userService.FindUserByID(userID) ?? throw new ArgumentException("Can not find user");
                    context.Items["User"] = user;
                }
            }

            await next(context);
        }
    }
}
