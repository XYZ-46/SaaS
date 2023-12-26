using AppConfiguration;
using InterfaceProject.IMiddleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Middleware.RabbitMQ
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private MessageRabbitMQConfig _messageConfig;

        public RabbitMQService(RabbitMQClientConfig clientConfig, MessageRabbitMQConfig messageConfig)
        {
            _messageConfig = messageConfig;
            var _clientConfig = clientConfig;
            var factory = new ConnectionFactory
            {
                HostName = _clientConfig.Hostnames,
                UserName = _clientConfig.Username,
                Password = _clientConfig.Password,
                Port = _clientConfig.Port
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public bool IsSameMessageConfig(MessageRabbitMQConfig messageConfig)
        {
            if (this._messageConfig.Queue == messageConfig.Queue &&
                this._messageConfig.ExchangeName == messageConfig.ExchangeName &&
                this._messageConfig.ExchangeType == messageConfig.ExchangeType &&
                this._messageConfig.RouteKey == messageConfig.RouteKey &&
                this._messageConfig.Durable == messageConfig.Durable &&
                this._messageConfig.AutoDelete == messageConfig.AutoDelete)
                return true;
            return false;
        }

        public void SetupQueue(MessageRabbitMQConfig messageConfig)
        {
            this._messageConfig = messageConfig;

            _channel.ExchangeDeclare(_messageConfig.ExchangeName, _messageConfig.ExchangeType);
            _channel.QueueDeclare(_messageConfig.Queue, true, false, false, null);
            _channel.QueueBind(_messageConfig.Queue, _messageConfig.ExchangeName, _messageConfig.RouteKey);
        }

        public void PushMessageIntoQueue(byte[] message)
        {
            _channel.BasicPublish(_messageConfig.ExchangeName, _messageConfig.RouteKey, null, message);
        }

        public void PushMessageIntoQueue(string message)
        {
            _channel.BasicPublish(_messageConfig.ExchangeName, _messageConfig.RouteKey, null, Encoding.UTF8.GetBytes(message));
        }

        public void PushMessageIntoQueue(object message)
        {
            _channel.BasicPublish(_messageConfig.ExchangeName, _messageConfig.RouteKey, null, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
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

        ~RabbitMQService() => Dispose(false);
    }
}
