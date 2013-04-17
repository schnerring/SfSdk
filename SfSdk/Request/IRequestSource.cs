using System.Collections.Generic;
using System.Threading.Tasks;
using SfSdk.Constants;
using SfSdk.Response;

namespace SfSdk.Request
{
    /// <summary>
    ///     Used to request data from a S&amp;F data source.
    /// </summary>
    internal interface IRequestSource
    {
        /// <summary>
        ///     Request the data asynchronously.
        /// </summary>
        /// <param name="sessionId">A valid session ID, with the length of 32. <see cref="Session.EmptySessionId" /> is used for logging in.</param>
        /// <param name="action">The action which shall be executed. See <see cref="SF" /> which start with "Act".</param>
        /// <param name="args">Additional arguments like e.g. the search string for searches or the user credentials for logging in.</param>
        /// <returns>The response as a <see cref="SfResponse"/>.</returns>
        Task<ISfResponse> RequestAsync(string sessionId, SF action, IEnumerable<string> args = null);
    }
}