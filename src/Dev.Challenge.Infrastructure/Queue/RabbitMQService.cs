using Dev.Challenge.Application.Queue;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Dev.Challenge.Infrastructure.Queue
{
    //public class RabbitMQService : IRabbitMQService, IDisposable
    //{
    //    private IConnection _connection;
    //    private IModel _channel;

    //    public RabbitMQService(IConfiguration configuration)
    //    {
    //        var factory = new ConnectionFactory
    //        {
    //            HostName = configuration["RabbitMQ:HostName"],
    //            UserName = configuration["RabbitMQ:UserName"],
    //            Password = configuration["RabbitMQ:Password"],
    //            VirtualHost = configuration["RabbitMQ:VirtualHost"]
    //        };

    //        _connection = factory.CreateConnection();
    //        _channel = _connection.CreateModel();
    //    }

    //    public void Publish<T>(T message, string queueName)
    //    {
    //        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
    //        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

    //        var properties = _channel.CreateBasicProperties();
    //        properties.Persistent = true;

    //        _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
    //    }

    //    public void Dispose()
    //    {
    //        _channel?.Close();
    //        _connection?.Close();
    //    }

    //    public IModel CreateChannel()
    //    {
    //        return _connection.CreateModel();
    //    }
    //}
    public class RabbitMQService : IRabbitMQService, IDisposable
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IConfiguration _configuration;
        private readonly object _lock = new object();

        public RabbitMQService(IConfiguration configuration)
        {
            _configuration = configuration;
            InitializeConnection();
        }

        private void InitializeConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"],
                VirtualHost = _configuration["RabbitMQ:VirtualHost"]
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as necessary
                Console.WriteLine($"Could not establish connection to RabbitMQ: {ex.Message}");
                Dispose();
                throw;
            }
        }

        public void Publish<T>(T message, string queueName)
        {
            lock (_lock)
            {
                EnsureConnectionIsOpen();

                _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;

                _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
            }
        }

        private void EnsureConnectionIsOpen()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                Dispose();
                InitializeConnection();
            }

            if (_channel == null || !_channel.IsOpen)
            {
                _channel = _connection.CreateModel();
            }
        }

        public IModel CreateChannel()
        {
            lock (_lock)
            {
                EnsureConnectionIsOpen();
                return _channel;
            }
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();

            _connection?.Close();
            _connection?.Dispose();
        }
    }

}
