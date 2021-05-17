using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQProject_ConvertFormatWordFile.Publisher.Models
{
    public class WordFile
    {
        public string Email { get; set; }
        public IFormFile File { get; set; }
    }
}
