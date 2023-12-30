using Microsoft.AspNetCore.Http;
using FluentAssertions;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using API.Logger;

namespace XunitTest.Middleware
{
    public class LoggerHttp_Test
    {
        [Fact]
        public async Task HttpRequest_Logger_Return_Success()
        {

            // Arrange 
            using (TestCorrelator.CreateContext())
            using (var logger = new LoggerConfiguration().WriteTo.Sink(new TestCorrelatorSink()).Enrich.FromLogContext().CreateLogger())
                Log.Logger = logger;

            HttpContext ctx = new DefaultHttpContext();
            static Task next(HttpContext hc) => Task.CompletedTask;

            // Act
            var mw = new LoggerReqHttp(next);
            ctx.Request.Headers.UserAgent = "Some Value";
            await mw.Invoke(ctx);

            //Assert
            ctx.Request.Headers.TryGetValue("User-Agent", out var value1).Should().Be(true);
            value1.Should().Equal("Some Value");

            ctx.Response.StatusCode.Should().Be(200);

            // test Log HttpContext
            var logEventsFromCurrentContext = TestCorrelator.GetLogEventsFromCurrentContext();
            logEventsFromCurrentContext.Should().ContainSingle().Which.MessageTemplate.Text.Should().Be("Request Http");

            var _properties = TestCorrelator.GetLogEventsFromCurrentContext().Select(x => x.Properties);
            _properties.Should().ContainSingle().Which.ContainsKey("InfoType");
            _properties.Should().ContainSingle().Which.ContainsKey("ReqBody");
            _properties.Should().ContainSingle().Which.ContainsKey("IpRemote");
            _properties.Should().ContainSingle().Which.ContainsKey("IpForwaded");
            _properties.Should().ContainSingle().Which.ContainsKey("Headers");
            _properties.Should().ContainSingle().Which.ContainsKey("ContentType");
            _properties.Should().ContainSingle().Which.ContainsKey("QueryString");
            _properties.Should().ContainSingle().Which.Values.Should().Contain(x => x.ToString().Contains("UserRequest"));
        }

        [Fact]
        public async Task HttpResponse_Logger_return_Success()
        {

            // Arrange 
            using (TestCorrelator.CreateContext())
            using (var logger = new LoggerConfiguration().WriteTo.Sink(new TestCorrelatorSink()).Enrich.FromLogContext().CreateLogger())
                Log.Logger = logger;

            HttpContext ctx = new DefaultHttpContext();
            static Task next(HttpContext hc) => Task.CompletedTask;

            // Act
            var mw = new LoggerRespHttp(next);
            ctx.Request.Headers.UserAgent = "Some Value";
            await mw.Invoke(ctx);

            //Assert
            ctx.Request.Headers.TryGetValue("User-Agent", out var value1).Should().Be(true);
            value1.Should().Equal("Some Value");

            ctx.Response.StatusCode.Should().Be(200);

            // test Log HttpContext
            var logEventsFromCurrentContext = TestCorrelator.GetLogEventsFromCurrentContext();
            logEventsFromCurrentContext.Should().HaveCount(2);
            logEventsFromCurrentContext.Should().Contain(x => x.MessageTemplate.ToString() == "Request Http");
            logEventsFromCurrentContext.Should().Contain(x => x.MessageTemplate.ToString() == "Response Http");

            var _properties = TestCorrelator.GetLogEventsFromCurrentContext().Select(x => x.Properties).ToArray();
            _properties.Should().HaveCount(2);
            
            // Cek Log Request
            var reqParam = _properties[0];
            reqParam.Should().Contain(x => x.Key == "InfoType");
            reqParam.Should().Contain(x => x.Key == "ReqBody");
            reqParam.Should().Contain(x => x.Key == "IpRemote");
            reqParam.Should().Contain(x => x.Key == "IpForwaded");
            reqParam.Should().Contain(x => x.Key == "Headers");
            reqParam.Should().Contain(x => x.Key == "ContentType");
            reqParam.Should().Contain(x => x.Key == "QueryString");

            reqParam.TryGetValue("InfoType", out var valInfoTypeReq0);
            valInfoTypeReq0?.ToString().Trim('"').Should().Be("UserRequest");

            reqParam.TryGetValue("QueryString", out var valInfoTypeReq1);
            valInfoTypeReq1?.ToString().Trim('"').Should().BeNullOrEmpty();

            reqParam.TryGetValue("ReqBody", out var valInfoTypeReq2);
            valInfoTypeReq2?.ToString().Trim('"').Should().BeNullOrEmpty();

            // Cek Log Response
            var resParam = _properties[1];
            resParam.Should().Contain(x => x.Key == "InfoType");
            resParam.Should().Contain(x => x.Key == "RespBody");
            resParam.Should().Contain(x => x.Key == "StatusCode");

            resParam.TryGetValue("InfoType", out var valInfoTypeRes0);
            valInfoTypeRes0?.ToString().Trim('"').Should().Be("UserResponse");

            resParam.TryGetValue("RespBody", out var valInfoTypeRes1);
            valInfoTypeRes1?.ToString().Trim('"').Should().BeNullOrEmpty();

            resParam.TryGetValue("StatusCode", out var valInfoTypeRes2);
            valInfoTypeRes2?.ToString().Trim('"').Should().Be("200");
        }
    }
}
