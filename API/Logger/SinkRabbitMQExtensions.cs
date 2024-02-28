using InterfaceProject.Service;
using Serilog;
using Serilog.Configuration;

namespace API.Logger
{
    public static class SinkRabbitMQExtensions
    {
        public static LoggerConfiguration SinkRabbitMQ(
                  this LoggerSinkConfiguration loggerConfiguration,
                  IRabbitMQService rabbitMQService,
                  bool IsProduction,
                  IFormatProvider? formatProvider = null)
        {
            return loggerConfiguration.Sink(new SinkRabbitMQ(rabbitMQService, formatProvider!, IsProduction));
        }
    }
}
