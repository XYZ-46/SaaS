using Microsoft.AspNetCore.Diagnostics;

namespace API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is null) return false;

            var response = new BaseResponse()
            {
                errorMessage = exception.Message,
            };

            switch (exception)
            {
                case ArgumentException:
                    httpContext.Response.StatusCode = 400;
                    await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
                    break;
                default:
                    httpContext.Response.StatusCode = 500;
                    response.errorMessage = "Internal server Error";
                    await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
                    break;
            }

            return true;
        }
    }
}
