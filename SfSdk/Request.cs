using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using SfSdk.Constants;
using SfSdk.Logging;

namespace SfSdk
{
    /// <summary>
    ///     Helps to build requests against S&amp;F servers easier.
    /// </summary>
    internal class Request
    {
        /// <summary>
        ///     The addressed request source, which is set to the <see cref="SnFRequestSource"/> per default.
        /// </summary>
        public static Func<Uri, Uri, IRequestSource> RequestSource =
            (serverUri, requestUri) => new SnFRequestSource(serverUri, requestUri);

        private static readonly ILog Log = LogManager.GetLog(typeof (Request));
        private static readonly Random Random;

        private readonly string _action;
        private readonly string _args;
        private readonly Guid _id;
        private readonly Uri _requestUri;
        private readonly Uri _serverUri;
        private readonly string _sessionId;

        static Request()
        {
            Random = new Random(DateTime.Now.Millisecond);
        }

        /// <summary>
        ///     Creates a instance of <see cref="Request" />.
        /// </summary>
        /// <param name="sessionId">A valid session ID, with the length of 32. <see cref="Session.EmptySessionId" /> is used for logging in.</param>
        /// <param name="serverUri">The server URI where the request is going to be received on.</param>
        /// <param name="action">The action which shall be executed. See <see cref="SF" /> which start with "Act".</param>
        /// <param name="args">Additional arguments like e.g. the search string for searches or the user credentials for logging in.</param>
        public Request(string sessionId, Uri serverUri, SF action, IEnumerable<string> args = null)
        {
            if (sessionId == null) throw new ArgumentNullException("sessionId");
            if (serverUri == null) throw new ArgumentNullException("serverUri");
            if (sessionId.Length != 32) throw new ArgumentException("SessionId must have a length of 32", "sessionId");

            _id = Guid.NewGuid();
            _sessionId = sessionId;
            _serverUri = serverUri;
            _action = ((int) action).ToString(CultureInfo.InvariantCulture);
            while (_action.Length < 3)
                _action = _action.Insert(0, "0");
            if (args != null) _args = string.Join(";", args);
            _requestUri = BuildRequestUri();
        }

        private Uri BuildRequestUri()
        {
            string url = string.Empty;
            url += _serverUri;
            url += "request.php?req=";
            url += _sessionId;
            url += _action;
            url += _args;
            url += "&rnd=";
            url += Random.Next(2000000000);
            url += Math.Round(DateTime.Now.ToUnixTimeStamp());
            return new UriBuilder(url).Uri;
        }

        /// <summary>
        ///     Executes the <see cref="Request"/> asynchronously.
        /// </summary>
        /// <returns>A <see cref="RequestResult"/> containing the result information.</returns>
        public async Task<RequestResult> ExecuteAsync()
        {
            try
            {
                Log.Info("Request started:  ID = {0}", _id);
                Log.Info("    SID:    {0}", _sessionId);
                Log.Info("    Action: {0}", _action);
                Log.Info("    Args:   {0}", _args ?? "null");
                Log.Info("    URL:    {0}", _requestUri.AbsoluteUri);

                var source = RequestSource(_serverUri, _requestUri);
                RequestResult result = await source.RequestAsync();

                Log.Info("Request finished: ID = {0}", _id);
                return result;
            }
            catch (WebException)
            {
                throw new NotImplementedException();
            }
        }
    }
}