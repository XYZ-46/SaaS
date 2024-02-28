using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IO;
using Serilog;

namespace API.Middleware
{
    public class LoggerResponseHttp : IMiddleware
    {
        private readonly RecyclableMemoryStreamManager _recyclableMem = new();

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            //await FormatResponse(context.Response);
            await LogResponse(context, next);
            //await next(context);

        }

        private async Task FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            Log
               .ForContext("InfoType", "UserResponse")
               .ForContext("RespBody", responseBody)
               .ForContext("StatusCode", response.StatusCode)
               .Information("Response Http");
        }

        private async Task LogResponse(HttpContext context, RequestDelegate next)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMem.GetStream();
            context.Response.Body = responseBody;
            await next(context);
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
