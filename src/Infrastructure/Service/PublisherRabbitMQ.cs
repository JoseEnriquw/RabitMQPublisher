using Core.Common.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Service
{
    public class PublisherRabbitMQ : IPublisherRabbitMQ
    {
        private readonly ILogger<PublisherRabbitMQ> _logger;
        private readonly RabbitMQOption _rabbitMQ;
        
        public PublisherRabbitMQ(ILogger<PublisherRabbitMQ> logger, RabbitMQOption rabbitMQ)
        {
            _logger = logger;
            _rabbitMQ = rabbitMQ;
        }

        public async Task Publish(object message, string queue)=>await Task.Run(()=>
        {
            var factory = new ConnectionFactory() 
            {
                HostName = _rabbitMQ.Host,
                UserName= _rabbitMQ.UserName,
                Password = _rabbitMQ.Password,
                Port = _rabbitMQ.Port
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queue,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

                var body = SerializeObject(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: queue,
                                     basicProperties: null,
                                     body: body);
                _logger.LogInformation($"Sent {message} to queue {queue}");
            }
        });

        private byte[] SerializeObject(object value) => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
    }
}
