using RabbitMQ.Client;

namespace Middleware.RabbitMQ
{
    public interface IRabbitMQConnectionFactory
    {
        IConnection CreateConnection();
    }
}
