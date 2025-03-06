namespace StockWatch.Filters
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string To {get;set;}
        public string CC { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string AppPassword {get;set;}
    }
}
