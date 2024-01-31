using API.Logger;
using AppConfiguration;
using InterfaceProject.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Repository.Database;
using Serilog;
using Service;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;
using System.Text;

namespace API
{
    [ExcludeFromCodeCoverage]
    public static partial class Program
    {
        public static void Main(string[] args)
        {
            string ASPNETCORE_ENVIRONMENT = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() ?? "production";
            IConfiguration _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{ASPNETCORE_ENVIRONMENT}.json", true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                .Build();

            var builder = WebApplication.CreateBuilder(args);
            { // Service
                builder.Services.AddSwaggerGen();

                var _jwtSetting = _config.GetSection("JwtSetting").Get<JwtSetting>();

                builder.Services.RegisterDIServices(_config);
                builder.Services.RegisterDIRepository();

                builder.Services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(opt => opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _jwtSetting!.Issuer,
                        ValidAudience = _jwtSetting.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Secret))
                    });

                builder.Services.AddDbContext<AzureDB>(options => options.UseSqlServer(_config.GetSection("Database:Azure").Value));
                builder.Services.AddControllers();

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

    } // End class Program
}









