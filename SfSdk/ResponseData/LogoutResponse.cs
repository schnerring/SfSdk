namespace SfSdk.ResponseData
{
    internal class LogoutResponse : ResponseBase
    {
        public bool LogoutSucceeded { get; private set; }
        public LogoutResponse(string[] args, bool logoutSucceeded) : base(args)
        {
            LogoutSucceeded = logoutSucceeded;
        }
    }
}