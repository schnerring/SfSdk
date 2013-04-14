using System;
using NLog;

namespace SfSdk.Logging
{
    internal class NLogLogger : ILog
    {
        private readonly Logger _logger;

        public NLogLogger(Type type)
        {
            _logger = NLog.LogManager.GetLogger(type.Name);
        }

        public void Info(string format, params object[] args)
        {
            _logger.Info(format, args);
        }

        public void Warn(string format, params object[] args)
        {
            _logger.Warn(format, args);
        }

        public void Error(Exception exception)
        {
            _logger.ErrorException(exception.Message, exception);
        }
    }
}