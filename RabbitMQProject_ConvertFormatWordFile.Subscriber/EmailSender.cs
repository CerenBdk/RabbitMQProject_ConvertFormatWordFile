using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace RabbitMQProject_ConvertFormatWordFile.Subscriber
{
    public class EmailSender
    {
        public bool EmailSendForPdf(string email, MemoryStream memoryStream, string fileName)
        {
            try
            {
                memoryStream.Position = 0;
                System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                Attachment attach = new Attachment(memoryStream, contentType);
                attach.ContentDisposition.FileName = $"{fileName}.pdf";

                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();

                mailMessage.From = new MailAddress("test@outlook.com");
                mailMessage.To.Add(email);
                mailMessage.Subject = "Pdf File";
                mailMessage.Body = "Your pdf file is attached";
                mailMessage.IsBodyHtml = true;
                mailMessage.Attachments.Add(attach);

                smtpClient.Host = "smtp-mail.outlook.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl= true;
                smtpClient.Credentials = new System.Net.NetworkCredential("test@outlook.com", "test123");
                smtpClient.Send(mailMessage);
                Console.WriteLine("Mail has been sended.");

                memoryStream.Close();
                memoryStream.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed: {ex.Message}");
                return false;
            }

        }

        public bool EmailSendForZip(string email, MemoryStream memoryStream, string fileName)
        {
            try
            {
                memoryStream.Position = 0;
                System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Zip);
                Attachment attach = new Attachment(memoryStream, contentType);
                attach.ContentDisposition.FileName = $"{fileName}.zip";

                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();

                mailMessage.From = new MailAddress("test@outlook.com");
                mailMessage.To.Add(email);
                mailMessage.Subject = "Zip File";
                mailMessage.Body = "Your zip file is attached";
                mailMessage.IsBodyHtml = true;
                mailMessage.Attachments.Add(attach);

                smtpClient.Host = "smtp-mail.outlook.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new System.Net.NetworkCredential("test@outlook.com", "test123");
                smtpClient.Send(mailMessage);
                Console.WriteLine("Mail has been sended.");

                memoryStream.Close();
                memoryStream.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed: {ex.Message}");
                return false;
            }

        }
    }
}
