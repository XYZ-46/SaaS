using AppConfiguration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Middleware.RabbitMQ
{
    public class RabbitMQConnectionFactoryService : IRabbitMQConnectionFactory
    {
        private readonly IConnection _connection;

        public RabbitMQConnectionFactoryService(IOptionsMonitor<RabbitMQClientConfig> clientConfig)
        {
            var _clientConfig = clientConfig.CurrentValue;
            var factory = new ConnectionFactory
            {
                HostName = _clientConfig.Hostnames,
                UserName = _clientConfig.Username,
                Password = _clientConfig.Password,
                Port = _clientConfig.Port

            };
            _connection = factory.CreateConnection();
        }

        public IConnection CreateConnection() => this._connection;
    }
}
