namespace Middleware.RabbitMQ
{
    public interface IRabbitMQService : IDisposable
    {
        public void CloseConnection();
        public void PublishDirect(string queueName, string RouteKey, string message);
        public void PublishFanout(string queueName, string message);
    }
}
