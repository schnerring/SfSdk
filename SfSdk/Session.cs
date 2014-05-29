using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SfSdk.Constants;
using SfSdk.Contracts;
using SfSdk.Data;
using SfSdk.Logging;
using SfSdk.Request;
using SfSdk.Response;

namespace SfSdk
{
    /// <summary>
    ///     A host of actions, which can be performed as you log the session in with valid user credentials. 
    /// </summary>
    internal class Session : ISession
    {
        private static readonly ILog Log = LogManager.GetLog(typeof (Session));

        private const string EmptySessionId = "00000000000000000000000000000000";

        private readonly Func<Uri, IRequestSource> _sourceFactory;

        internal Uri ServerUri { get; private set; }

        private string _sessionId;
        private IRequestSource _source;
        private bool _isLoggedIn;
        private string _username;
        private string _md5PasswordHash;

        /// <summary>
        ///     Creates a new instance of type <see cref="Session"/> querying the default <see cref="SnFRequestSource"/>.
        /// </summary>
        internal Session() : this(serverUri => new SnFRequestSource(serverUri))
        {
        }

        /// <summary>
        ///     Creates a new instance of type <see cref="Session"/>.
        /// </summary>
        /// <param name="sourceFactory">The factory which overrides the default behaviour with a <see cref="SnFRequestSource"/>.</param>
        internal Session(Func<Uri, IRequestSource> sourceFactory)
        {
            _sourceFactory = sourceFactory;
        }

        /// <summary>
        ///     The mushrooms count the currently logged in session.
        /// </summary>
        public int Mushrooms { get; private set; }

        /// <summary>
        ///     The gold count the currently logged in session.
        /// </summary>
        public int Gold { get; private set; }

        /// <summary>
        ///     The silver count the currently logged in session.
        /// </summary>
        public int Silver { get; private set; }

        /// <summary>
        ///     Logs the current session in.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="md5PasswordHash">The MD5 hash of the password.</param>
        /// <param name="serverUri">The <see cref="Uri"/> of the server to be logged on.</param>
        /// <returns>The success of the login process as <see cref="bool"/>.</returns>
        /// <exception cref="ArgumentException">When username or password hash have invalid formats.</exception>
        /// <exception cref="ArgumentNullException">When serverUri is null.</exception>
        public async Task<bool> LoginAsync(string username, string md5PasswordHash, Uri serverUri)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username must not be null or empty.", "username");
            if (md5PasswordHash == null || md5PasswordHash.Length != 32) 
                throw new ArgumentException("Password hash must not be null and have a length of 32.", "md5PasswordHash");
            if (serverUri == null)
                throw new ArgumentNullException("serverUri");
            
            _isLoggedIn = false;
            _username = username;
            _md5PasswordHash = md5PasswordHash;
            ServerUri = serverUri;
            _source = _sourceFactory(ServerUri);
            var result =
                await
                new SfRequest().ExecuteAsync(_source, EmptySessionId, SF.ActLogin,
                                             new[] { _username, _md5PasswordHash, "v1.70&random=%2" });

            var hasErrors = HasErrors(result.Errors);
            var response = result.Response as LoginResponse;
            if (response == null || hasErrors) return _isLoggedIn;

            _sessionId = response.SessionId;
            Mushrooms = response.Mushrooms;
            Gold = response.Gold;
            Silver = response.Silver;

            _isLoggedIn = true;
            return _isLoggedIn;
        }

        /// <summary>
        ///     Logs the current session out.
        /// </summary>
        /// <returns>The success of the logout as <see cref="bool"/>.</returns>
        /// <exception cref="SessionLoggedOutException">When session is not logged in.</exception>
        public async Task<bool> LogoutAsync()
        {
            if (!_isLoggedIn) throw new SessionLoggedOutException("LogoutAsync requires to be logged in.");
            var result = await new SfRequest().ExecuteAsync(_source, _sessionId, SF.ActLogout);
            var response = result.Response as LogoutResponse;
            var hasErrors = HasErrors(result.Errors);
            _isLoggedIn = response == null || hasErrors;
            return !_isLoggedIn;
        }

        /// <summary>
        ///     Represents the Character Screen Action.
        /// </summary>
        /// <returns>The <see cref="ICharacter"/> of the currently logged in account.</returns>
        /// <exception cref="SessionLoggedOutException">When session is not logged in.</exception>
        public async Task<ICharacter> MyCharacterAsync()
        {
            if (!_isLoggedIn) throw new SessionLoggedOutException("MyCharacterAsync requires to be logged in.");
            var result = await new SfRequest().ExecuteAsync(_source, _sessionId, SF.ActScreenChar);
            var hasErrors = HasErrors(result.Errors);
            var characterResponse = result.Response as ICharacterResponse;
            return hasErrors || characterResponse == null
                ? null
                : new Character(characterResponse, _username, this, ServerUri);
        }

        /// <summary>
        ///     Requests a Character via a given predicate.
        /// </summary>
        /// <param name="username">The username to search.</param>
        /// <returns>The <see cref="ICharacter"/> if the name could be found, null if not.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="SessionLoggedOutException">When session is not logged in.</exception>
        public async Task<ICharacter> RequestCharacterAsync(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (!_isLoggedIn) throw new SessionLoggedOutException("RequestCharacterAsync requires to be logged in.");
            var result = await new SfRequest().ExecuteAsync(_source, _sessionId, SF.ActRequestChar, new[] { username });
            var hasErrors = HasErrors(result.Errors);
            var characterResponse = result.Response as ICharacterResponse;
            return hasErrors || characterResponse == null
                       ? null
                       : new Character(characterResponse, username, this, ServerUri);
        }

        /// <summary>
        ///     Represents the Hall Of Fame Screen Action.
        /// </summary>
        /// <param name="searchString">Search strings may contain the rank or the name of a character to be searched.</param>
        /// <param name="forceLoad">Indicates whether the details of the characters shall be loaded.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> where T: <see cref="ICharacter"/>/>.</returns>
        /// <exception cref="SessionLoggedOutException">When session is not logged in.</exception>
        public async Task<IEnumerable<ICharacter>> HallOfFameAsync(string searchString = null, bool forceLoad = false)
        {
            if (!_isLoggedIn) throw new SessionLoggedOutException("HallOfFameAsync requires to be logged in.");

            string[] args = null;
            if (searchString != null)
            {
                int rank;
                args =
                    int.TryParse(searchString, out rank)
                        ? new[] {string.Empty, rank.ToString(CultureInfo.InvariantCulture)}
                        : new[] {searchString, (-1).ToString(CultureInfo.InvariantCulture)};
            }

            var result = await new SfRequest().ExecuteAsync(_source, _sessionId, SF.ActScreenEhrenhalle, args);
            
            var hasErrors = HasErrors(result.Errors);
            var response = result.Response as IHallOfFameResponse;
            if (hasErrors || response == null) return null;
            return
                response.Characters
                        .Select(c => new Character(c.Item1, c.Item2, c.Item3, c.Item4, c.Item5, this))
                        .Cast<ICharacter>()
                        .ToList();

        }

        /// <summary>
        ///     Represents the Album Action.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{T}"/> where T: <see cref="IScrapbookItem"/>/>.</returns>
        /// <exception cref="SessionLoggedOutException">When session is not logged in.</exception>
        public async Task<IEnumerable<IScrapbookItem>> ScrapbookAsync()
        {
            if (!_isLoggedIn) throw new SessionLoggedOutException("ScrapbookAsync requires to be logged in.");

            var result = await new SfRequest().ExecuteAsync(_source, _sessionId, SF.ActAlbum);
            var hasErrors = HasErrors(result.Errors);
            var response = result.Response as IScrapbookResponse;
            
            if (hasErrors || response == null) return null;
            return response.Items;
        }

        private bool HasErrors(IReadOnlyCollection<SF> errors)
        {
            var hasErrors = false;
            if (errors.Count == 0) return false;
            foreach (var e in errors)
            {
                hasErrors = true;
                switch (e)
                {
                    case SF.ErrSessionIdExpired:
                        throw new SessionLoggedOutException("Sessionid expired.");
                    case SF.ErrLoginFailed:
                        break;
                    case SF.ErrNoAlbum:
                        throw new NotImplementedException("There is no album.");
                    default:
                        var ex =
                            new NotImplementedException(
                                string.Format("This error is not handled: {0}.", e));
                        Log.Error(ex);
                        throw ex;
                }
            }
            return hasErrors;
        }
    }
}