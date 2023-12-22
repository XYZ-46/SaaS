using AppConfiguration;
using Microsoft.Extensions.Options;
using Middleware.RabbitMQ;
using Serilog.Core;
using Serilog.Events;
using System.Text.Json;

namespace Middleware.Logger
{
    public class SinkRabbitMQ
        : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider;
        private readonly IRabbitMQService _rabbitMQService;
        private readonly SinkRabbitMQConfig _sinkRabbitmqConfig;

        public SinkRabbitMQ(IRabbitMQService rabbitMQService, IFormatProvider formatProvider, SinkRabbitMQConfig sinkConfig)
        {
            _sinkRabbitmqConfig = sinkConfig;
            _formatProvider = formatProvider;
            _rabbitMQService = rabbitMQService;
        }

        public void Emit(LogEvent logEvent)
        {
            var objLog = new Dictionary<string, string>
            {
                { "date", logEvent.Timestamp.UtcDateTime.ToString("o") }
            };

            foreach (var properti in logEvent.Properties) objLog.Add(properti.Key, properti.Value.ToString());
            objLog.Add("message", logEvent.RenderMessage(_formatProvider));

            _rabbitMQService.PublishMessage(objLog, _sinkRabbitmqConfig.Queue);

            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var properti in objLog)
            {
                Console.WriteLine($"{properti.Key} = {properti.Value}");
            }

        }
    }
}
