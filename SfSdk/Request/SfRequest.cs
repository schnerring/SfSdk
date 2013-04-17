using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SfSdk.Constants;
using SfSdk.Logging;
using SfSdk.Response;

namespace SfSdk.Request
{
    /// <summary>
    ///     Helps to build requests against S&amp;F servers easier.
    /// </summary>
    internal class SfRequest
    {
        private static readonly ILog Log = LogManager.GetLog(typeof (SfRequest));

        /// <summary>
        ///     Executes the <see cref="SfRequest" /> asynchronously.
        /// </summary>
        /// <param name="source">The addressed request source, which should be an instance of type <see cref="SnFRequestSource" /> in usual cases.</param>
        /// <param name="sessionId">A valid session ID, with the length of 32. <see cref="Session.EmptySessionId" /> is used for logging in.</param>
        /// <param name="action">The action which shall be executed. See <see cref="SF" /> which start with "Act".</param>
        /// <param name="args">Additional arguments like e.g. the search string for searches or the user credentials for logging in.</param>
        /// <returns>A <see cref="SfResponse" /> containing the result information.</returns>
        public async Task<ISfResponse> ExecuteAsync(IRequestSource source, string sessionId, SF action, IEnumerable<string> args = null)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (sessionId == null) throw new ArgumentNullException("sessionId");
            if (sessionId.Length != 32) throw new ArgumentException("SessionId must have a length of 32.", "sessionId");

            var id = Guid.NewGuid();

            Log.Info("Request started:  ID = {0}", id);
            var response = await source.RequestAsync(sessionId, action, args);
            Log.Info("Request finished: ID = {0}", id);
            
            return response;
        }
    }
}