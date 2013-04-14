// ------------------------------------------------------------------------------
// Copyright (c) 2010 Blue Spire Consulting, Inc.
// CURRENT: Caliburn.Micro Caliburn.Micro v1.5.1
// DATE:    Fri Mar 22, 2013 at 9:00 AM
// URL:     http://caliburnmicro.codeplex.com/
// ------------------------------------------------------------------------------

using System;

namespace SfSdk.Logging
{
    /// <summary>
    ///     A logger.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        ///     Logs the message as info.
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        void Info(string format, params object[] args);

        /// <summary>
        ///     Logs the message as a warning.
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        void Warn(string format, params object[] args);

        /// <summary>
        ///     Logs the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        void Error(Exception exception);
    }

    /// <summary>
    ///     Used to manage logging.
    /// </summary>
    internal static class LogManager
    {
        private static readonly ILog NullLogInstance = new NullLog();

        /// <summary>
        ///     Creates an <see cref="ILog" /> for the provided type.
        /// </summary>
//        public static Func<Type, ILog> GetLog = type => NullLogInstance;
        public static Func<Type, ILog> GetLog = type => new NLogLogger(type);

        private class NullLog : ILog
        {
            public void Info(string format, params object[] args)
            {
            }

            public void Warn(string format, params object[] args)
            {
            }

            public void Error(Exception exception)
            {
            }
        }
    }
}