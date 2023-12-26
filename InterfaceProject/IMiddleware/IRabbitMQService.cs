using AppConfiguration;

namespace InterfaceProject.IMiddleware
{
    public interface IRabbitMQService : IDisposable
    {
        bool IsSameMessageConfig(MessageRabbitMQConfig messageConfig);
        void SetupQueue(MessageRabbitMQConfig messageConfig);
        void PushMessageIntoQueue(byte[] message);
        void PushMessageIntoQueue(string message);
        void PushMessageIntoQueue(object message);
    }
}
