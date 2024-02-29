using Microsoft.AspNetCore.Http.Features;
using Serilog;
using System.Text;
using System.Text.Json;

namespace API.Middleware
{
    public class RequestResponseLogger : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Request.EnableBuffering();

            await LogRequest(context);
            await LogResponse(context, next);
        }

        public async Task LogRequest(HttpContext context)
        {
            IHttpRequestFeature features = context.Features.Get<IHttpRequestFeature>();
            string url = $"{features?.Scheme}://{context.Request.Host.Value}{features?.RawTarget}";

            IFormCollection form = null;
            string formString = string.Empty;

            if (context.Request.HasFormContentType) form = context.Request.Form;
            else
            {
                formString = await new StreamReader(context.Request.Body).ReadToEndAsync();
                var injectedRequestStream = new MemoryStream();
                byte[] bytesToWrite = Encoding.UTF8.GetBytes(formString);
                injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                injectedRequestStream.Seek(0, SeekOrigin.Begin);
                context.Request.Body = injectedRequestStream;
            }

            Log
              .ForContext("InfoType", "UserRequest")
              .ForContext("urlRequest", url)
              .ForContext("IpRemote", context.Connection.RemoteIpAddress?.ToString())
              .ForContext("IpForwaded", context.Request.Headers["X-Forwarded-For"].ToString())
              .ForContext("Headers", JsonSerializer.Serialize(context.Request.Headers))
              .ForContext("ContentType", context.Request.ContentType)
              .ForContext("QueryString", context.Request.QueryString)
              .ForContext("FormData", (form?.Count > 0) ? string.Empty : JsonSerializer.Serialize(form))
              .ForContext("ReqBody", formString)
              .Information("Request Http");

            //return new Logging.AuditLog()
            //{
            //    RemoteHost = context.Connection.RemoteIpAddress.ToString(),
            //    HttpURL = url,
            //    LocalAddress = context.Connection.LocalIpAddress.ToString(),
            //    Headers = Newtonsoft.Json.JsonConvert.SerializeObject(context.Request.Headers),
            //    Form = form != null ? Newtonsoft.Json.JsonConvert.SerializeObject(form) : formString
            //};
        }

        public async Task LogResponse(HttpContext context, RequestDelegate _next)
        {
            Stream originalBody = context.Response.Body;

            try
            {

                using var memStream = new MemoryStream();
                context.Response.Body = memStream;

                await _next(context);

                memStream.Position = 0;
                string responseBody = new StreamReader(memStream).ReadToEnd();

                Log
                    .ForContext("InfoType", "UserResponse")
                    .ForContext("RespBody", responseBody)
                    .ForContext("StatusCode", context.Response.StatusCode)
                    .Information("Response Http");

                memStream.Position = 0;
                await memStream.CopyToAsync(originalBody);
            }
            //catch
            //{
            //    auditLog?.Save();
            //    throw;
            //}
            finally
            {
                context.Response.Body = originalBody;
            }
        }
    }
}
