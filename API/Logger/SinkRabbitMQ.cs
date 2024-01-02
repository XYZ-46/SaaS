using InterfaceProject.Service;
using Serilog.Core;
using Serilog.Events;

namespace API.Logger
{
    public class SinkRabbitMQ(IRabbitMQService rabbitMQService, IFormatProvider formatProvider, bool IsProduction )
                : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider = formatProvider;
        private readonly IRabbitMQService _rabbitMQService = rabbitMQService;

        public void Emit(LogEvent logEvent)
        {
            var objLog = new Dictionary<string, string>
            {
                { "date", logEvent.Timestamp.UtcDateTime.ToString("o") },
                { "level", logEvent.Level.ToString() }
            };

            if (logEvent.TraceId.HasValue) objLog.Add("TraceId", logEvent.TraceId.Value.ToString());
            if (logEvent.SpanId.HasValue) objLog.Add("SpanId", logEvent.SpanId.Value.ToString());

            foreach (var properti in logEvent.Properties) objLog.Add(properti.Key, properti.Value.ToString().Trim('"'));
            objLog.Add("message", logEvent.RenderMessage(_formatProvider));

            _rabbitMQService.PushMessageIntoQueue(objLog);

            if (!IsProduction)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                foreach (var properti in objLog)
                {
                    Console.WriteLine($"{properti.Key} = {properti.Value}");
                }
            }

        }
    }
}
