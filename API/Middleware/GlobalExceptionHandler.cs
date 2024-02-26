using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is null) return ValueTask.FromResult(false);

            switch (exception)
            {
                case ArgumentException:
                case HttpRequestException:
                    BaseResponse response = new();
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.errorMessage = exception.Message;
                    httpContext.Response.WriteAsJsonAsync(exception.Message, cancellationToken).ConfigureAwait(false);
                    break;
                default:
                    response = new BaseResponse();
                    httpContext.Response.StatusCode = StatusCodes.Status506VariantAlsoNegotiates;
                    response.errorMessage = "Undefined server Error";
                    httpContext.Response.WriteAsJsonAsync(JsonSerializer.Serialize(response), cancellationToken);
                    break;
            }

            return ValueTask.FromResult(true);
        }
    }
}
