using SfSdk.ResponseData;

namespace SfSdk
{
    internal class LogoutResponse : IResponse
    {
        public bool LogoutSucceeded { get; private set; }
        public LogoutResponse(bool logoutSucceeded)
        {
            LogoutSucceeded = logoutSucceeded;
        }
    }
}