using CoreOnion_Backend.Application.Interfaces.RabbitMQServices;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Infrastructure.Services
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly string _hostName = "aaaaaaaaaaaaaaaaaaa";
        private readonly string _queueName = "mailQueue";

        public void SendMessage(object message)
        {
            var factory = new ConnectionFactory() { Uri = new Uri(_hostName) };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: messageBody);

        }
    }
}
