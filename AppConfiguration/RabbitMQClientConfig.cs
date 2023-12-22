using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfiguration
{
    public class RabbitMQClientConfig
    {
        public string Hostnames { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Port { get; set; } = 5672;
        public string VHost { get; set; } = "/";
        public ushort Heartbeat { get; set; } = 0;
    }
}
