using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 using MimeKit;
 
namespace StockWatch.Models
{
   


    public class Message
    {
        public string To { get; set; }
        public string? CC { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public List<IFormFile>? Attachments { get; set; }
        public Message(string subject, string content)
        {
            Subject = subject;
            Content = content;
        }
    }
}
