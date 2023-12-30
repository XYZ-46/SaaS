using InterfaceProject.Service;
using Serilog.Core;
using Serilog.Events;

namespace API.Logger
{
    public class SinkRabbitMQ(IRabbitMQService rabbitMQService, IFormatProvider formatProvider)
                : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider = formatProvider;
        private readonly IRabbitMQService _rabbitMQService = rabbitMQService;

        public void Emit(LogEvent logEvent)
        {
            var objLog = new Dictionary<string, string>
            {
                { "date", logEvent.Timestamp.UtcDateTime.ToString("o") }
            };

            foreach (var properti in logEvent.Properties) objLog.Add(properti.Key, properti.Value.ToString().Trim('"'));
            objLog.Add("message", logEvent.RenderMessage(_formatProvider));

            _rabbitMQService.PushMessageIntoQueue(objLog);

            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var properti in objLog)
            {
                Console.WriteLine($"{properti.Key} = {properti.Value}");
            }

        }
    }
}
