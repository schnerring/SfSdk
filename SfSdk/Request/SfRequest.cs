using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SfSdk.Constants;
using SfSdk.DataSource;
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
        ///     The addressed request source, which is set to the <see cref="SnFRequestSource" /> per default.
        /// </summary>
        public static Func<IUriFactory, IRequestSource> RequestSource =
            uriBuilder => new SnFRequestSource(uriBuilder as SnFUriFactory);

        /// <summary>
        ///     The <see cref="IUriFactory" /> for the corresponding <see cref="IRequestSource" />, 
        ///     which is set to the <see cref="SnFUriFactory" /> per default.
        /// </summary>
        public static Func<string, Uri, SF, IEnumerable<string>, IUriFactory> UriFactory =
            (sessionId, serverUri, action, args) => new SnFUriFactory(sessionId, serverUri, action, args);

        private readonly Guid _id;
        private readonly IUriFactory _uriFactory;

        /// <summary>
        ///     Creates a instance of <see cref="SfRequest" />.
        /// </summary>
        /// <param name="sessionId">A valid session ID, with the length of 32. <see cref="Session.EmptySessionId" /> is used for logging in.</param>
        /// <param name="serverUri">The server URI where the request is going to be received on.</param>
        /// <param name="action">The action which shall be executed. See <see cref="SF" /> which start with "Act".</param>
        /// <param name="args">Additional arguments like e.g. the search string for searches or the user credentials for logging in.</param>
        public SfRequest(string sessionId, Uri serverUri, SF action, IEnumerable<string> args = null)
        {
            if (sessionId == null) throw new ArgumentNullException("sessionId");
            if (serverUri == null) throw new ArgumentNullException("serverUri");
            if (sessionId.Length != 32) throw new ArgumentException("SessionId must have a length of 32.", "sessionId");

            _id = Guid.NewGuid();
            _uriFactory = UriFactory(sessionId, serverUri, action, args);
        }

        /// <summary>
        ///     Executes the <see cref="SfRequest" /> asynchronously.
        /// </summary>
        /// <returns>A <see cref="SfResponse" /> containing the result information.</returns>
        public async Task<SfResponse> ExecuteAsync()
        {
            Log.Info("Request started:  ID = {0}", _id);
            Log.Info("    SID:    {0}", _uriFactory.SessionId);
            Log.Info("    Action: {0}", _uriFactory.Action);
            Log.Info("    Args:   {0}", _uriFactory.Args ?? "null");
            Log.Info("    URL:    {0}", _uriFactory.RequestUri.AbsoluteUri);

            IRequestSource source = RequestSource(_uriFactory);
            SfResponse response = await source.RequestAsync();

            Log.Info("Request finished: ID = {0}", _id);
            return response;
        }
    }
}