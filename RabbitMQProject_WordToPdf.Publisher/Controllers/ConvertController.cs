using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQProject_ConvertFormatWordFile.Publisher.Models;
using RabbitMQProject_ConvertFormatWordFile.Publisher.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQProject_ConvertFormatWordFile.Publisher.Controllers
{
    public class ConvertController : Controller
    {
        RabbitMQPublisher _publisher;
        private readonly ILogger<ConvertController> _logger;

        public ConvertController(RabbitMQPublisher publisher, ILogger<ConvertController> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public IActionResult WordToPdfPage()
        {
            return View();
        }

        public IActionResult WordToZipPage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult WordToPdfPage(WordFile wordFile)
        {
            _publisher.Publish(wordFile);
            ViewBag.result = "After converting the word file to a pdf file, it will be sent to your email address.";
            return View();
        }

        [HttpPost]
        public IActionResult WordToZipPage(WordFile wordFile)
        {
            _publisher.Publish(wordFile);
            ViewBag.result = "After converting the word file to a zip file, it will be sent to your email address.";
            return View();
        }
    }
}
