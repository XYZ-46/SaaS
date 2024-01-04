using Serilog;
using System.Reflection;
using System.Net;
using AppConfiguration;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Service;
using API.Logger;
using InterfaceProject.Service;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Repository.Database;

namespace API
{
    [ExcludeFromCodeCoverage]
    public static partial class Program
    {
        public static void Main(string[] args)
        {
            string ASPNETCORE_ENVIRONMENT = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() ?? "production";
            var _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{ASPNETCORE_ENVIRONMENT}.json", true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                .Build();

            var builder = WebApplication.CreateBuilder(args);
            { // Service
                builder.Services.AddSwaggerGen();

                var _jwtSetting = new JwtSetting();
                _config.Bind("JwtSetting", _jwtSetting);
                builder.Services.AddSingleton(Options.Create(_jwtSetting));

                builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();
                builder.Services.AddTransient<IUserService, UserService>();
                builder.Services.AddTransient<IAuthService, AuthService>();

                builder.Services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(opt => opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _jwtSetting.Issuer,
                        ValidAudience = _jwtSetting.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Secret))
                    });

                builder.Services.AddDbContext<AzureDB>(options => options.UseSqlServer(_config.GetSection("Database:Azure").ToString()));

                builder.Services.AddControllers();
                builder.Services.AddSingleton<IRabbitMQService>(x =>
                {
                    var _rabbitmqClientConfig = _config.GetSection("Middleware:RabbitMQClient").Get<RabbitMQClientConfig>();
                    var _sinkMessageConfig = _config.GetSection("Serilog:SinkRabbitMQ").Get<MessageRabbitMQConfig>();

                    var rabbitService = new RabbitMQService(_rabbitmqClientConfig!, _sinkMessageConfig!);
                    return rabbitService;
                });

                builder.Host.UseSerilog((hostBuilderContext, service, loggerConfig) =>
                {
                    loggerConfig
                        .ReadFrom.Configuration(hostBuilderContext.Configuration)
                        .Enrich.WithProperty("ENV", ASPNETCORE_ENVIRONMENT)
                        .Enrich.WithProperty("version", _config.GetSection("version").Value)
                        .Enrich.WithProperty("ApplicationName", _config.GetSection("ApplicationName").Value)
                        .WriteTo.SinkRabbitMQ(rabbitMQService: service.GetRequiredService<IRabbitMQService>(), IsProduction: ASPNETCORE_ENVIRONMENT == "production");
                });
            }

            var app = builder.Build();
            { // App Builder
                app.UseMiddleware<LoggerReqHttp>();
                app.UseMiddleware<LoggerRespHttp>();
                app.UseMiddleware<ErrorHandler>();
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
            }

        } // End public static void Main

        static string GetLocalIPAddress()
        {
            List<string> addressIPs = [];
            foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                addressIPs.Add(address.ToString());

            return string.Join("; ", addressIPs.Where(x => !string.IsNullOrEmpty(x)));
        }

    } // End public static partial class Program
}









