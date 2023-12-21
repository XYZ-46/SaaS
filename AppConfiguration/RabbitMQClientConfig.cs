using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfiguration
{
    public class RabbitMQClientConfig
    {
        public string Hostnames { get; set; } = "localhost";
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Exchange { get; set; } = string.Empty;
        public string ExchangeType { get; set; } = string.Empty;
        public bool Durable { get; set; } = true;
        public bool AutoDelete { get; set; } = false;
        public string RouteKey { get; set; } = string.Empty;
        public string RouteKeyLogger { get; set; } = string.Empty;
        public string QueueLogger { get; set; } = string.Empty;
        public string ExchangeLogger { get; set; } = string.Empty;
        public int Port { get; set; } = 5672;
        public string VHost { get; set; } = string.Empty;
        public ushort Heartbeat { get; set; }
    }
}
