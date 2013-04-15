using System;
using System.Collections.Generic;
using System.Globalization;
using SfSdk.Constants;

namespace SfSdk.Request
{
    /// <summary>
    ///     A helper class to assemble a S&amp;F request <see cref="Uri"/> more easily.
    /// </summary>
    internal class SnFUriFactory : IUriFactory
    {
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        private readonly string _sessionId;
        private readonly Uri _serverUri;
        private readonly string _action;
        private readonly string _args;
        private readonly Uri _requestUri;

        /// <summary>
        ///     Creates a instance of <see cref="SnFUriFactory" />.
        /// </summary>
        /// <param name="sessionId">A valid session ID, with the length of 32. <see cref="Session.EmptySessionId" /> is used for logging in.</param>
        /// <param name="serverUri">The server URI where the request is going to be received on.</param>
        /// <param name="action">The action which shall be executed. See <see cref="SF" /> which start with "Act".</param>
        /// <param name="args">Additional arguments like e.g. the search string for searches or the user credentials for logging in.</param>
        public SnFUriFactory(string sessionId, Uri serverUri, SF action, IEnumerable<string> args = null)
        {
            if (sessionId == null) throw new ArgumentNullException("sessionId");
            if (serverUri == null) throw new ArgumentNullException("serverUri");
            if (sessionId.Length != 32) throw new ArgumentException("SessionId must have a length of 32.", "sessionId");

            _sessionId = sessionId;
            _serverUri = serverUri;
            _action = ((int) action).ToString(CultureInfo.InvariantCulture);
            while (_action.Length < 3)
                _action = _action.Insert(0, "0");
            if (args != null) _args = String.Join(";", args);
            _requestUri = BuildRequestUri();
        }

        private Uri BuildRequestUri()
        {
            string url = String.Empty;
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

        public string SessionId
        {
            get { return _sessionId; }
        }

        public Uri ServerUri
        {
            get { return _serverUri; }
        }

        public string Action
        {
            get { return _action; }
        }

        public string Args
        {
            get { return _args; }
        }

        public Uri RequestUri
        {
            get { return _requestUri; }
        }
    }
}