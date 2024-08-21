using RabbitMQ.Client;

namespace Dev.Challenge.Application.Queue
{
    public interface IRabbitMQService
    {
        void Publish<T>(T message, string queueName);
        IModel CreateChannel();
    }
}
