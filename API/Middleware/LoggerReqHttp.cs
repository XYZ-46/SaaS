using Microsoft.IO;
using Serilog;
using System.Text.Json;

namespace API.Middleware
{
    public class LoggerReqHttp : IMiddleware
    {
        private readonly RecyclableMemoryStreamManager _recyclableMem = new();

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await LogRequest(context);
            await next(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            await using var requestStream = _recyclableMem.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            Log
                .ForContext("InfoType", "UserRequest")
                .ForContext("IpRemote", context.Connection.RemoteIpAddress)
                .ForContext("IpForwaded", context.Request.Headers["X-Forwarded-For"].ToString())
                .ForContext("Headers", JsonSerializer.Serialize(context.Request.Headers))
                .ForContext("ContentType", context.Request.ContentType)
                .ForContext("QueryString", context.Request.QueryString)
                .ForContext("ReqBody", ReadStreamInChunks(requestStream))
                .Information("Request Http");
            context.Request.Body.Position = 0;
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);
            return textWriter.ToString();
        }
    }
}
