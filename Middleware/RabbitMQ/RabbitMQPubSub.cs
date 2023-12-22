using AppConfiguration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Middleware.RabbitMQ
{
    public class RabbitMQPubSub
    {
        private readonly IModel _model;
        private readonly MessageRabbitMQConfig _messageConfig;

        public RabbitMQPubSub(IModel model, MessageRabbitMQConfig messageConfig)
        {
            _model = model;
            _messageConfig = messageConfig;
        }


        public void SetupQueue()
        {
            _model.ExchangeDeclare(_messageConfig.ExchangeName, _messageConfig.ExchangeType);
            _model.QueueDeclare(_messageConfig.Queue, true, false, false, null);
            _model.QueueBind(_messageConfig.Queue, _messageConfig.ExchangeName, _messageConfig.RouteKey);
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

        //public byte[] ReadMessageFromQueue(string queueName)
        //{
        //    SetupQueue();
        //    byte[] message;
        //    var data = _model.BasicGet(queueName, false);
        //    message = data.Body;
        //    _model.BasicAck(data.DeliveryTag, false);
        //    return message;
        //}

    }
}
