using Microsoft.AspNetCore.Diagnostics;
using System.Security.Authentication;

namespace API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is null) return false;

            httpContext.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Json;
            BaseResponse resp = new();

            switch (exception)
            {
                case ArgumentException:
                case HttpRequestException:
                case AuthenticationException:
                    resp.errorMessage = exception.Message;
                    httpContext.Response.StatusCode = StatusCodes.Status417ExpectationFailed;
                    await httpContext.Response.WriteAsJsonAsync(resp, cancellationToken: cancellationToken);
                    break;
                default:
                    httpContext.Response.StatusCode = StatusCodes.Status501NotImplemented;
                    resp.errorMessage = "Undefined Error";
                    await httpContext.Response.WriteAsJsonAsync(resp, cancellationToken: cancellationToken);
                    break;
            }

            return false;
        }
    }
}
