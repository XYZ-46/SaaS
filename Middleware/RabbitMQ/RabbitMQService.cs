using AppConfiguration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Middleware.RabbitMQ
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private RabbitMQClientConfig _rabbitClientConfig;
        private MessageRabbitMQConfig _sinkRabbitMQConfig;

        public RabbitMQService()
        {

            var factory = new ConnectionFactory()
            {
                HostName = _rabbitClientConfig!.Hostnames, // RabbitMQ server address
                Port = _rabbitClientConfig.Port,          // Default port
                UserName = _rabbitClientConfig.Username,  // Default username
                Password = _rabbitClientConfig.Password   // Default password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void SetupQueue(string queueName)
        {
            _channel.ExchangeDeclare(_sinkRabbitMQConfig.ExchangeName, _sinkRabbitMQConfig.ExchangeType);
            _channel.QueueDeclare(queueName, true, false, false, null);
            _channel.QueueBind(queueName, _sinkRabbitMQConfig.ExchangeName, _sinkRabbitMQConfig.RouteKey);
        }

        public void PublishMessage(byte[] message, string queue)
        {
            SetupQueue(queue);
            _channel.BasicPublish(_sinkRabbitMQConfig.ExchangeName, _sinkRabbitMQConfig.RouteKey, null, message);
        }

        public void PublishMessage(string message, string queue)
        {
            SetupQueue(queue);
            _channel.BasicPublish(_sinkRabbitMQConfig.ExchangeName, _sinkRabbitMQConfig.RouteKey, null, Encoding.UTF8.GetBytes(message));
        }

        public void PublishMessage(object message, string queue)
        {
            SetupQueue(queue);
            _channel.BasicPublish(_sinkRabbitMQConfig.ExchangeName, _sinkRabbitMQConfig.RouteKey, null, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _channel.Dispose();
            _connection.Dispose();
        }

        ~RabbitMQService()
        {
            Dispose(false);
        }
    }
}
