using SfSdk.Constants;

namespace SfSdk.Response
{
    /// <summary>
    ///     The reponse type returned on <see cref="SF.RespLogoutSuccess" />.<br />
    ///     Triggered by action <see cref="SF.ActLogout" />.
    /// </summary>
    internal interface ILogoutResponse
    {
    }

    /// <summary>
    ///     The reponse type returned on <see cref="SF.RespLogoutSuccess" />.<br />
    ///     Triggered by action <see cref="SF.ActLogout" />.
    /// </summary>
    internal class LogoutResponse : ResponseBase, ILogoutResponse
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