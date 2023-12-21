using Middleware.RabbitMQ;
using Serilog.Core;
using Serilog.Events;
using System.Text.Json;

namespace Middleware.Logger
{
    public class SinkRabbitMQ(IRabbitMQService rabbitMQService, IFormatProvider formatProvider) : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider = formatProvider;
        private readonly IRabbitMQService _rabbitMQService = rabbitMQService;

        public void Emit(LogEvent logEvent)
        {
            var objLog = new Dictionary<string, string>
            {
                { "date", logEvent.Timestamp.UtcDateTime.ToString("o") }
            };

            foreach (var properti in logEvent.Properties)
            {
                objLog.Add(properti.Key, properti.Value.ToString());
            }
            objLog.Add("message", logEvent.RenderMessage(_formatProvider));

            _rabbitMQService.PublishFanout("LogApps-exchange", JsonSerializer.Serialize(objLog));

            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var properti in objLog)
            {
                Console.WriteLine($"{properti.Key} = {properti.Value}");
            }

        }
    }
}
