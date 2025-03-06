
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using StockWatch.Filters;
using StockWatch.Models;
using StockWatch.Services.Abstract;

namespace StockWatch.Services.Contract
{
    public class EmailProvider : IEmailProvider
    {
        public readonly EmailConfiguration _emailConfig;
        public IConfiguration Configuration { get; }
        public EmailProvider(IOptions<EmailConfiguration> emailConfig, IConfiguration configuration)
        {
            _emailConfig = emailConfig.Value;
            Configuration = configuration;
        }


        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.Cc.Add(new MailboxAddress("",_emailConfig.CC));
            emailMessage.From.Add(new MailboxAddress(_emailConfig.DisplayName, _emailConfig.From));
            emailMessage.To.Add(new MailboxAddress("Recipient Name", _emailConfig.To));
            emailMessage.Subject = message.Subject;

            var builder = new BodyBuilder();
            
            builder.HtmlBody = message.Content;
            

            emailMessage.Body = builder.ToMessageBody();
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.Username, _emailConfig.AppPassword);
                    Console.WriteLine(mailMessage);

                    client.Send(mailMessage);
                }

                catch(SystemException ex)
                {
                    throw ex;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

    }
}