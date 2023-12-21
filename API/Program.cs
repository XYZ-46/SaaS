using Middleware.RabbitMQ;
using Serilog;
using System.Reflection;
using Middleware.Logger;
using System.Net;
using AppConfiguration;

var _config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddEnvironmentVariables()
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
        .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
        .Build();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.Configure<RabbitMQClientConfig>(_config.GetSection("Middleware:SinkRabbitMQ"));
builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();

builder.Host.UseSerilog((hostBuilderContext, service, loggerConfig) =>
{
    loggerConfig
        .ReadFrom.Configuration(hostBuilderContext.Configuration)
        .Enrich.WithProperty("ENV", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
        .Enrich.WithProperty("version", _config.GetSection("version").Value)
        .Enrich.WithProperty("ApplicationName", _config.GetSection("ApplicationName").Value)
        .WriteTo.SinkRabbitMQ(service.GetRequiredService<IRabbitMQService>());

});

var app = builder.Build();
app.UseMiddleware<LoggerReqHttp>();
app.UseMiddleware<LoggerRespHttp>();
if (!app.Environment.IsEnvironment("Production"))
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
Log
    .ForContext("IsDevelopment", app.Environment.IsDevelopment())
    .ForContext("IPLocal", GetLocalIPAddress())
    .ForContext("app.Environment.EnvironmentName", app.Environment.EnvironmentName)
    .ForContext("app.Environment.ApplicationName", app.Environment.ApplicationName)
    .Information("Program Start");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

static string GetLocalIPAddress()
{
    List<string> addressIPs = [];
    foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
    {
        addressIPs.Add(address.ToString());
    }

    return string.Join("; ", addressIPs.Where(x => !string.IsNullOrEmpty(x)));
}
