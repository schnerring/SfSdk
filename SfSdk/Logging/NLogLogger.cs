using System;
using NLog;

namespace SfSdk.Logging
{
    /// <summary>
    ///     A logger implementing NLog.
    /// </summary>
    public class NLogLogger : ILog
    {
        private readonly Logger _logger;

        /// <summary>
        ///     Creates an instance of the <see cref="NLogLogger"/>.
        /// </summary>
        /// <param name="type">The type of the class which shall be logged.</param>
        public NLogLogger(Type type)
        {
            _logger = NLog.LogManager.GetLogger(type.Name);
        }

        /// <summary>
        ///     Logs the message as info.
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        public void Info(string format, params object[] args)
        {
            _logger.Info(format, args);
        }

        /// <summary>
        ///     Logs the message as a warning.
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        public void Warn(string format, params object[] args)
        {
            _logger.Warn(format, args);
        }

        /// <summary>
        ///     Logs the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void Error(Exception exception)
        {
            _logger.ErrorException(exception.Message, exception);
        }
    }
}