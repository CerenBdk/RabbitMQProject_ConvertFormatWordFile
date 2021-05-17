using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQProject_ConvertFormatWordFile.Publisher.Services
{
    public class RabbitMQClientService : IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private readonly ILogger<RabbitMQClientService> _logger;
        public static string exchangeName = "convert-exchange";
        public static string queueNameForPdf = "queue-convert-pdf";
        public static string routeKeyForPdf = "route-convert-pdf";
        public static string queueNameForZip = "queue-convert-zip";
        public static string routeKeyForZip = "route-convert-zip";

        public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();
            if (_channel is { IsOpen: true })
            {
                return _channel;
            }

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true, false, null);
            _channel.QueueDeclare(queueNameForPdf, true, false, false, null);
            _channel.QueueBind(queueNameForPdf, exchangeName, routeKeyForPdf);
            _channel.QueueDeclare(queueNameForZip, true, false, false, null);
            _channel.QueueBind(queueNameForZip, exchangeName, routeKeyForZip);


            _logger.LogInformation("Connection with RabbitMQ has been established.");
            return _channel;
        }
        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("Connection with RabbitMQ has been lost.");
        }
    }
}
