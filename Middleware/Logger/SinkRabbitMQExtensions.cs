using Serilog.Configuration;
using Serilog;
using Middleware.RabbitMQ;
using AppConfiguration;
using Microsoft.Extensions.Options;

namespace Middleware.Logger
{
    public static class SinkRabbitMQExtensions
    {
        public static LoggerConfiguration SinkRabbitMQ(
                  this LoggerSinkConfiguration loggerConfiguration,
                  IRabbitMQConnectionFactory rabbitMQService,
                  Action<SinkRabbitMQConfig> configure,
                  IFormatProvider? formatProvider = null)
        {
            var sinkConfig = new SinkRabbitMQConfig();
            configure(sinkConfig);
            return loggerConfiguration.Sink(new SinkRabbitMQ(rabbitMQService, formatProvider,sinkConfig));
        }
    }
}
