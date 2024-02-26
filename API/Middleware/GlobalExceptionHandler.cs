using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is null) return false;

            switch (exception)
            {
                case ArgumentException:
                case HttpRequestException:
                    BaseResponse response = new();
                    httpContext.Response.StatusCode = StatusCodes.Status417ExpectationFailed;
                    response.errorMessage = exception.Message;
                    await Task.Run(() =>
                    {
                        _ = httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
                    }, cancellationToken);
                    break;
                default:
                    response = new BaseResponse();
                    httpContext.Response.StatusCode = StatusCodes.Status506VariantAlsoNegotiates;
                    response.errorMessage = "Undefined server Error";
                    await httpContext.Response.WriteAsJsonAsync(JsonSerializer.Serialize(response), cancellationToken);
                    break;
            }

            return true;
        }
    }
}
