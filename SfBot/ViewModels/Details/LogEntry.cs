using System;

namespace SfBot.ViewModels.Details
{
    public class LogEntry
    {
        public LogEntry(string message)
        {
            Time = DateTime.Now;
            Message = message;
        }

        public DateTime Time { get; private set; }
        public string Message { get; private set; }
    }
}