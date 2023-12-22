
namespace AppConfiguration
{
    public class MessageRabbitMQConfig
    {
        public string Queue { get; set; } = string.Empty;
        public string ExchangeName { get; set; } = string.Empty;
        public string ExchangeType { get; set; } = string.Empty;
        public string RouteKey { get; set; } = string.Empty;
        public bool Durable { get; set; } = true;
        public bool AutoDelete { get; set; } = false;
    }
}
