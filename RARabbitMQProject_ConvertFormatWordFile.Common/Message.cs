using System;

namespace RARabbitMQProject_ConvertFormatWordFile.Common
{
    public class Message
    {
        public byte[] WordByte { get; set; }
        public string Email { get; set; }
        public string FileName { get; set; }
    }
}
