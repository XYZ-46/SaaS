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
            var resp = new BaseResponse();
            httpContext.Response.ContentType = "application/json";

            switch (exception)
            {
                case ArgumentException:
                case HttpRequestException:
                case AuthenticationException:
                    resp.errorMessage = exception.Message;
                    httpContext.Response.StatusCode = StatusCodes.Status417ExpectationFailed;
                    break;

                case System.ComponentModel.DataAnnotations.ValidationException:
                case FluentValidation.ValidationException:
                    if (httpContext.Response.StatusCode == 415) resp.errorMessage = "Invalid Json format";

                    break;
                default:
                    httpContext.Response.StatusCode = StatusCodes.Status501NotImplemented;
                    resp.errorMessage = "Undefined Error";
                    break;
            }
            await httpContext.Response.WriteAsJsonAsync(resp, cancellationToken: cancellationToken);

            return false;
        }
    }
}
