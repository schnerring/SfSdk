using System;
using System.Linq;
using System.Threading.Tasks;
using SfSdk.Constants;
using SfSdk.Contracts;
using SfSdk.Data;
using SfSdk.Logging;
using SfSdk.ResponseData;

namespace SfSdk
{
    public class Session : ISession
    {
        private static readonly ILog Log = LogManager.GetLog(typeof (Session));

        private Uri _serverUri;
        private string _sessionId;

        public int Mushrooms { get; private set; }
        public int Gold { get; private set; }
        public int Silver { get; private set; }

        /// <summary>
        ///     Logs the current session in.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="md5PasswordHash">The MD5 hash of the password.</param>
        /// <param name="serverUri">The Uri of the server to be logged on.</param>
        /// <returns>The success of the login as bool.</returns>
        public async Task<bool> LoginAsync(string username, string md5PasswordHash, Uri serverUri)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (md5PasswordHash == null) throw new ArgumentNullException("md5PasswordHash");
            if (serverUri == null) throw new ArgumentNullException("serverUri");

            const string emptySessionId = "00000000000000000000000000000000";
            _serverUri = serverUri;
            var request = new Request(emptySessionId, _serverUri, SF.ActLogin,
                                      new[] {username, md5PasswordHash, "v1.70&random=%2"});

            var result = await request.ExecuteAsync();

            var response = result.Response as LoginResponse;
            if (result.Errors.Count > 0 || response == null) return false;

            _sessionId = response.SessionId;
            Mushrooms = response.Mushrooms;
            Gold = response.Gold;
            Silver = response.Silver;

            return true;
        }

        /// <summary>
        ///     Logs out the current session.
        /// </summary>
        /// <returns>The success of the logout as bool.</returns>
        public async Task<bool> LogoutAsync()
        {
            var result = await new Request(_sessionId, _serverUri, SF.ActLogout).ExecuteAsync();
            if (result.Errors.Count > 1)
            {
                return result.Errors.Any(e => e == "SessionIdExpired");
            }
            var response = result.Response as LogoutResponse;
            if (response == null) return false;
            return response.LogoutSucceeded;
        }

        /// <summary>
        ///     Represents the Character Screen Action.
        /// </summary>
        /// <returns>The character of the currently logged in account.</returns>
        public async Task<ICharacter> CharacterScreenAsync()
        {
            var request = new Request(_sessionId, _serverUri, SF.ActScreenChar);
            var result = await request.ExecuteAsync();
            return new Character(result.Response as CharacterResponse);
        }

        /// <summary>
        ///     Requests a Character via a given predicate.
        /// </summary>
        /// <param name="username">The username to search.</param>
        /// <returns>Null if the name cannot be found.</returns>
        public async Task<ICharacter> RequestCharacterAsync(string username)
        {
            var request = new Request(_sessionId, _serverUri, SF.ActRequestChar, new[] { username });
            var result = await request.ExecuteAsync();
            return new Character(result.Response as CharacterResponse);
        }

        /// <summary>
        ///     Represents the Hall Of Fame Screen Action.
        /// </summary>
        /// <param name="forceLoad">Indicates whether the details of the characters shall be loaded.</param>
        /// <returns>A list of ICharacter.</returns>
        public async Task<ICharacter> HallOfFameAsync(bool forceLoad = false)
        {
            var request = new Request(_sessionId, _serverUri, SF.ActScreenEhrenhalle);
            var result = await request.ExecuteAsync();
            return new Character(result.Response as CharacterResponse);
        }
    }
}