using System;

namespace SfSdk
{
    internal class SessionLoggedOutException : Exception
    {
        public SessionLoggedOutException(string message) : base(message)
        {
        }
    }
}