using RabbitMQ.Client;

namespace Middleware.RabbitMQ
{
    public class RabbitMQClientConfiguration
    {
        public string Hostnames { get; set; } = "localhost";
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Exchange { get; set; } = string.Empty;
        public string ExchangeType { get; set; } = string.Empty;
        public bool Durable { get; set; } = true;
        public bool AutoDelete { get; set; } = false;
        public string RouteKey { get; set; } = string.Empty;
        public int Port { get; set; } = 5672;
        public string VHost { get; set; } = string.Empty;
        public IProtocol Protocol { get; set; }
        public ushort Heartbeat { get; set; }
        public SslOption SslOption { get; set; }

        public RabbitMQClientConfiguration From(RabbitMQClientConfiguration config)
        {
            Hostnames = config.Hostnames;
            Username = config.Username;
            Password = config.Password;
            Exchange = config.Exchange;
            ExchangeType = config.ExchangeType;
            Durable = config.Durable;
            RouteKey = config.RouteKey;
            Port = config.Port;
            VHost = config.VHost;
            Protocol = config.Protocol;
            Heartbeat = config.Heartbeat;
            SslOption = config.SslOption;

            return this;
        }
    }
}
