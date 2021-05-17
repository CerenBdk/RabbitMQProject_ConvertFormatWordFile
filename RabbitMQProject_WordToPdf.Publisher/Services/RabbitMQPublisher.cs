using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQProject_ConvertFormatWordFile.Publisher.Models;
using RARabbitMQProject_ConvertFormatWordFile.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQProject_ConvertFormatWordFile.Publisher.Services
{
    public class RabbitMQPublisher
    {
        private readonly RabbitMQClientService _rabbitMQClientService;

        public RabbitMQPublisher(RabbitMQClientService rabbitMQClientService)
        {
            _rabbitMQClientService = rabbitMQClientService;
        }

        public void Publish(WordFile wordFile)
        {
            var channel =_rabbitMQClientService.Connect();

            Message message = new Message();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wordFile.File.CopyTo(memoryStream);
                message.WordByte = memoryStream.ToArray();
            }

            message.Email = wordFile.Email;
            message.FileName = Path.GetFileNameWithoutExtension(wordFile.File.FileName);
            var serializeMessage = JsonConvert.SerializeObject(message);
            var messageBody = Encoding.UTF8.GetBytes(serializeMessage);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(RabbitMQClientService.exchangeName, RabbitMQClientService.routeKeyForPdf,properties, messageBody);
            channel.BasicPublish(RabbitMQClientService.exchangeName, RabbitMQClientService.routeKeyForZip, properties, messageBody);
        }
    }
}
