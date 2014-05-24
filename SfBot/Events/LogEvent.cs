using SfBot.Data;
using SfSdk;

namespace SfBot.Events
{
    public class LogEvent
    {
        public Account Account { get; set; }
        public string Message { get; set; }

        public LogEvent(Account account, string message)
        {
            Account = account;
            Message = message;
        }
    }
}