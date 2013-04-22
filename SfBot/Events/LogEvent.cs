using SfSdk;

namespace SfBot.Events
{
    public class LogEvent
    {
        public Session Session { get; set; }
        public string Message { get; set; }

        public LogEvent(Session session, string message)
        {
            Session = session;
            Message = message;
        }
    }
}