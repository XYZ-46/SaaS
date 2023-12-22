using AppConfiguration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Middleware.RabbitMQ;
using Serilog.Core;
using Serilog.Events;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Middleware.Logger
{
    public class SinkRabbitMQ(IRabbitMQConnectionFactory rabbitMQService, IFormatProvider formatProvider, SinkRabbitMQConfig sinkConfig)
                : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider = formatProvider;
        private readonly IRabbitMQConnectionFactory _rabbitMQService = rabbitMQService;
        private readonly SinkRabbitMQConfig _sinkRabbitmqConfig = sinkConfig;

        public void Emit(LogEvent logEvent)
        {
            var objLog = new Dictionary<string, string>
            {
                { "date", logEvent.Timestamp.UtcDateTime.ToString("o") }
            };

            foreach (var properti in logEvent.Properties) objLog.Add(properti.Key, properti.Value.ToString());
            objLog.Add("message", logEvent.RenderMessage(_formatProvider));

            var _rabbitConnection = _rabbitMQService.CreateConnection();
            using (var model = _rabbitConnection.CreateModel())
            {
                var helper = new RabbitMQPubSub(model, _sinkRabbitmqConfig);
                helper.PushMessageIntoQueue(objLog);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var properti in objLog)
            {
                Console.WriteLine($"{properti.Key} = {properti.Value}");
            }

        }
    }
}
