using Dev.Challenge.Application.Queue;
using Dev.Challenge.Domain.Entities;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Dev.Challenge.Infrastructure.Consumer
{


    public class MotorcycleNotificationService : IHostedService
    {
        private readonly IModel _channel;
        private readonly string _queueName = "MotorcycleQueue";
        private EventingBasicConsumer _consumer;

        public MotorcycleNotificationService(IRabbitMQService rabbitMQService)
        {
            _channel = rabbitMQService.CreateChannel(); // Cria o canal RabbitMQ
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Declara a fila e configura o consumidor
            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var motorcycle = JsonSerializer.Deserialize<MotorcycleEntity>(message);

                // Verifica se o ano da moto é "2024"
                if (motorcycle != null && motorcycle.Year == 2024)
                {
                    // Realiza a notificação
                    Console.WriteLine($"Notification: A motorcycle from the year 2024 was detected. Model: {motorcycle.Model}");
                    // Ação adicional, como enviar notificação, etc.
                }

                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: _consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Fecha o canal e a conexão quando o serviço parar
            _channel?.Close();
            return Task.CompletedTask;
        }
    }

}
