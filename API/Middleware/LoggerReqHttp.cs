using Serilog;
using System.Net;
using System.Text;
using System.Text.Json;

namespace API.Middleware
{
    public class LoggerReqHttp : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await LogRequest(context.Request, context.Connection.RemoteIpAddress);
            await next(context);
        }

        private static async Task LogRequest(HttpRequest request, IPAddress? ipRequest)
        {
            request.EnableBuffering();
            var body = request.Body;

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            Log
               .ForContext("InfoType", "UserRequest")
               .ForContext("IpRemote", ipRequest)
               .ForContext("IpForwaded", request.Headers["X-Forwarded-For"].ToString())
               .ForContext("Headers", JsonSerializer.Serialize(request.Headers))
               .ForContext("ContentType", request.ContentType)
               .ForContext("QueryString", request.QueryString)
               .ForContext("ReqBody", bodyAsText)
               .Information("Request Http");
        }
    }
}
