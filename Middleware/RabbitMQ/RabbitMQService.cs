using AppConfiguration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace Middleware.RabbitMQ
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly RabbitMQClientConfig _rabbitClientConfig;

        public RabbitMQService(IOptionsMonitor<RabbitMQClientConfig> clientConfig)
        {
            _rabbitClientConfig = clientConfig.CurrentValue;
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitClientConfig.Hostnames, // RabbitMQ server address
                Port = _rabbitClientConfig.Port,          // Default port
                UserName = _rabbitClientConfig.Username,  // Default username
                Password = _rabbitClientConfig.Password   // Default password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void CloseConnection()
        {
            if (_channel.IsOpen) _channel.Close();
            if (_connection.IsOpen) _connection.Close();
        }

        public void PublishDirect(string queueName, string RouteKey, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(queueName, RouteKey, null, body);
        }

        public void PublishFanout(string queueName, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(queueName, string.Empty, null, body);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            CloseConnection();
            _channel.Dispose();
            _connection.Dispose();
        }

        ~RabbitMQService()
        {
            Dispose(false);
        }
    }
}
