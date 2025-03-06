
using StockWatch.Models;

namespace StockWatch.Services.Abstract
{
    public interface IEmailProvider
    {
        void SendEmail(Message message);
    }
}