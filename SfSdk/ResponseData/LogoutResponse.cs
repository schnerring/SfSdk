using SfSdk.Constants;

namespace SfSdk.ResponseData
{
    /// <summary>
    ///     The reponse type returned on <see cref="SF.ActScreenChar" />, <see cref="SF.RespLogoutSuccess" />.
    /// </summary>
    internal class LogoutResponse : ResponseBase
    {
        /// <summary>
        ///     Creates a new logout response.
        /// </summary>
        /// <param name="args">The response arguments.</param>
        public LogoutResponse(string[] args) : base(args)
        {
        }
    }
}