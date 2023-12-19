namespace Middleware.RabbitMQ
{
    public interface IRabbitMQService : IDisposable
    {
        public void CloseConnection();
        public void SendMessage(string queueName, string message);
        public void ReceiveMessage(string queueName, Action<string> messageHandler);
    }
}
