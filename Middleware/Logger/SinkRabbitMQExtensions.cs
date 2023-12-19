using Serilog.Configuration;
using Serilog;
using Middleware.RabbitMQ;

namespace Middleware.Logger
{
    public static class SinkRabbitMQExtensions
    {
        public static LoggerConfiguration SinkRabbitMQ(
                  this LoggerSinkConfiguration loggerConfiguration,
                  IRabbitMQService rabbitMQService,
                  IFormatProvider? formatProvider = null)
        {
            return loggerConfiguration.Sink(new SinkRabbitMQ(rabbitMQService, formatProvider));
        }
    }
}
