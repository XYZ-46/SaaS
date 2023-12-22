namespace Middleware.RabbitMQ
{
    public interface IRabbitMQService : IDisposable
    {
        void PublishMessage(byte[] message, string queue);
        void PublishMessage(string message, string queue);
        void PublishMessage(object message, string queue);
    }
}
