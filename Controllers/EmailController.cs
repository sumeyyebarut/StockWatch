using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using StockWatch.Models;
using StockWatch.Services.Abstract;

namespace EmailService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailProvider _emailProvider;

        public EmailsController(IEmailProvider emailProvider)
        {
            _emailProvider = emailProvider;
        }

        [HttpPost("Send")]
        public IActionResult SendMail([FromForm] SendMailDto mail)
        {
            var message = new Message( mail.Subject, mail.Content);
            _emailProvider.SendEmail(message);
            return Ok();
        }
    }
}