using AppConfiguration;
using InterfaceProject.Service;
using InterfaceProject.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterDIServices(this IServiceCollection services, IConfiguration _config)
        {
            var _jwtSetting = _config.GetSection("JwtSetting").Get<JwtSetting>();
            var _rabbitmqClientConfig = _config.GetSection("RabbitMQ:RabbitMQClient").Get<RabbitMQClientConfig>();
            var _sinkMessageConfig = _config.GetSection("RabbitMQ:SinkConfig").Get<MessageRabbitMQConfig>();

            return services
                .AddScoped<IJwtTokenService, JwtTokenService>(x => new JwtTokenService(_jwtSetting!))
                .AddScoped<IUserService, UserService>()
                .AddScoped<IAuthService, AuthService>()

                .AddSingleton<IRedisService, RedisService>(x => new RedisService(_config.GetSection("RedisConnection").Value!))
                .AddSingleton<IRabbitMQService>(x => new RabbitMQService(_rabbitmqClientConfig!, _sinkMessageConfig!));
        }
    }
}
