using Louman.Models.DTOs.Client;
using Louman.Models.DTOs.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Louman.Services
{
    public class MailingService : IMailingService
    {
        private readonly IConfiguration _configuration;
        private readonly Models.DTOs.Email.MailSettings _mailSettings;
        private readonly IWebHostEnvironment _env;
        public MailingService(IConfiguration configuration, IOptions<Models.DTOs.Email.MailSettings> mailSettings,IWebHostEnvironment env)
        {
            _configuration = configuration;
            _mailSettings = mailSettings.Value;
            _env = env;

        }
        

        public async Task SendEmailAsync(ClientDto client)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(client.Email));
            email.Subject = "Welcome Email";
            var builder = new BodyBuilder();


            var filePath = Path.Combine(_env.ContentRootPath,"wwwroot\\images", "louman.jpeg");

           
            var image = builder.LinkedResources.Add(filePath);
            image.ContentId = MimeUtils.GenerateMessageId();
            builder.HtmlBody = @$"Dear {client.Initials}, <br><br>
                                Welcome to the Louman System! We are so happy you joined us and we hope that our<br>
                                application will be of great service to you.If you have anyt question / queuries regarding products or<br>
                                any other thing, don't be shy to message us!<br><br>
                                Kind regards,<br> 
                                The Louman Team.<br><img height=""400px"" width=""400px"" src=""cid:{image.ContentId}"">";
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

    }
}
