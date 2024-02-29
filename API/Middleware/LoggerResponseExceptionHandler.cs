using Microsoft.AspNetCore.Diagnostics;
using Serilog;

namespace API.Middleware
{
    public class LoggerResponseExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is null) return false;

            Stream originalBody = httpContext.Response.Body;

            try
            {
                using var memStream = new MemoryStream();
                httpContext.Response.Body = memStream;

                memStream.Position = 0;
                string responseBody = new StreamReader(memStream).ReadToEnd();

                Log
                    .ForContext("Exception", exception.Message)
                    .ForContext("InfoType", "UserResponse Exception")
                    .ForContext("RespBody", responseBody)
                    .ForContext("StatusCode", httpContext.Response.StatusCode)
                    .Information("Response Http");

                memStream.Position = 0;
                await memStream.CopyToAsync(originalBody, cancellationToken);
            }
            finally
            {
                httpContext.Response.Body = originalBody;
            }

            return true;
        }
    }
}
