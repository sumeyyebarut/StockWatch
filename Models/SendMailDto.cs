namespace StockWatch.Models
{
    public class SendMailDto
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        public string ToMails { get; set; }   
        public string CC { get; set; }
    }
}