using Microsoft.IO;
using Serilog;

namespace API.Middleware
{
    public class LoggerRespHttp(RequestDelegate requestProcess)
    {
        private readonly RequestDelegate requestProcess = requestProcess;
        private readonly RecyclableMemoryStreamManager _recyclableMem = new();

        public async Task Invoke(HttpContext context)
        {
            await LogResponse(context);
        }

        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMem.GetStream();
            context.Response.Body = responseBody;
            await requestProcess(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var ResponseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            Log
                .ForContext("InfoType", "UserResponse")
                .ForContext("RespBody", ResponseText)
                .ForContext("StatusCode", context.Response.StatusCode)
                .Information("Response Http");
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
