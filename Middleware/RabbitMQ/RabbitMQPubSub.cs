using AppConfiguration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Middleware.RabbitMQ
{
    public class RabbitMQPubSub(IModel model, MessageRabbitMQConfig messageConfig)
    {
        private readonly IModel _model = model;
        private readonly MessageRabbitMQConfig _messageConfig = messageConfig;

        public void SetupQueue(MessageRabbitMQConfig? msgConfig = null)
        {
            msgConfig ??= this._messageConfig;

            _model.ExchangeDeclare(msgConfig.ExchangeName, msgConfig.ExchangeType);
            _model.QueueDeclare(msgConfig.Queue, true, false, false, null);
            _model.QueueBind(msgConfig.Queue, msgConfig.ExchangeName, msgConfig.RouteKey);
        }

        public void PushMessageIntoQueue(byte[] message)
        {
            SetupQueue();
            _model.BasicPublish(_messageConfig.ExchangeName, _messageConfig.RouteKey, null, message);
        }

        public void PushMessageIntoQueue(string message)
        {
            SetupQueue();
            _model.BasicPublish(_messageConfig.ExchangeName, _messageConfig.RouteKey, null, Encoding.UTF8.GetBytes(message));
        }

        public void PushMessageIntoQueue(object message)
        {
            SetupQueue();
            _model.BasicPublish(_messageConfig.ExchangeName, _messageConfig.RouteKey, null, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
        }

        public string? ReadMessageFromQueue(MessageRabbitMQConfig msgConfig)
        {
            SetupQueue(msgConfig);
            string message;
            var data = _model.BasicGet(msgConfig.Queue, false);

            if (data != null) return null;

            message = Encoding.UTF8.GetString(data!.Body.ToArray());
            _model.BasicAck(data.DeliveryTag, false);
            return message;
        }

    }
}
