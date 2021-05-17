using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQProject_ConvertFormatWordFile.Publisher.Services;
using RARabbitMQProject_ConvertFormatWordFile.Common;
using Spire.Doc;
using System;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace RabbitMQProject_ConvertFormatWordFile.Subscriber
{
    class Program
    {
        private static IModel _channel;
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://nxtkeaig:VF284EF4WjRGAy4ChuzTDz7xcp6Wibe-@baboon.rmq.cloudamqp.com/nxtkeaig");

            using var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(_channel);
            SubscribeForZip(consumer);
        }
        public static void SubscribeForPdf(EventingBasicConsumer consumer)
        {
            bool result = false;

            _channel.QueueBind(RabbitMQClientService.queueNameForPdf, RabbitMQClientService.exchangeName, RabbitMQClientService.routeKeyForPdf);
            _channel.BasicConsume(RabbitMQClientService.queueNameForPdf, false, consumer);

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                try
                {
                    Document document = new Document();

                    var messageStr = Encoding.UTF8.GetString(e.Body.ToArray());
                    Message message = JsonConvert.DeserializeObject<Message>(messageStr);
                    document.LoadFromStream(new MemoryStream(message.WordByte), FileFormat.Docx2013);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        EmailSender emailSender = new EmailSender();
                        document.SaveToStream(memoryStream, FileFormat.PDF);
                        result = emailSender.EmailSendForPdf(message.Email, memoryStream, message.FileName);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occured: {ex.Message}");
                }
                if (result)
                {
                    Console.WriteLine("The message in the queue has been processed successfully.");
                    _channel.BasicAck(e.DeliveryTag, false);
                }
            };
            Console.ReadLine();
        }

        public static void SubscribeForZip(EventingBasicConsumer consumer)
        {
            bool result = false;
            _channel.QueueBind(RabbitMQClientService.queueNameForZip, RabbitMQClientService.exchangeName, RabbitMQClientService.routeKeyForZip);
            _channel.BasicConsume(RabbitMQClientService.queueNameForZip, false, consumer);

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {

                try
                {
                    Document document = new Document();

                    var messageStr = Encoding.UTF8.GetString(e.Body.ToArray());
                    Message message = JsonConvert.DeserializeObject<Message>(messageStr);
                    document.LoadFromStream(new MemoryStream(message.WordByte), FileFormat.Docx2013);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        EmailSender emailSender = new EmailSender();
                        document.SaveToStream(memoryStream, FileFormat.Xml);
                        result = emailSender.EmailSendForZip(message.Email, memoryStream, message.FileName);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occured: {ex.Message}");
                }
                if (result)
                {
                    Console.WriteLine("The message in the queue has been processed successfully.");
                    _channel.BasicAck(e.DeliveryTag, false);
                }
            };

        }
    }
}
