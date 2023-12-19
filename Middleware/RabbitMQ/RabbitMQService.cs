using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Middleware.RabbitMQ
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly RabbitMQClientConfiguration _clientConfiguration;

        public RabbitMQService(IOptions<RabbitMQClientConfiguration> clientConfig)
        {
            _clientConfiguration = clientConfig.Value;
            var factory = new ConnectionFactory()
            {
                HostName = _clientConfiguration.Hostnames, // RabbitMQ server address
                Port = _clientConfiguration.Port,          // Default port
                UserName = _clientConfiguration.Username,  // Default username
                Password = _clientConfiguration.Password   // Default password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void CloseConnection()
        {
            if (_channel.IsOpen) _channel.Close();
            if (_connection.IsOpen) _connection.Close();
        }

        public void SendMessage(string queueName, string message)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: _clientConfiguration.Exchange, routingKey: _clientConfiguration.RouteKey, basicProperties: null, body: body);
        }

        public void ReceiveMessage(string queueName, Action<string> messageHandler)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                messageHandler.Invoke(message);
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
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
