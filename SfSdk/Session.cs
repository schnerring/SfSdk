using System;
using System.Linq;
using System.Threading.Tasks;
using SfSdk.Contracts;
using SfSdk.Enums;
using SfSdk.RequestData;

namespace SfSdk
{
    public class Session : ISession
    {
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
            var request = new Request(emptySessionId, _serverUri, SfAction.Login,
                                        new[] {username, md5PasswordHash, "v1.70&random=%2"});
            RequestResult result = await request.ExecuteAsync();

            var session = result.Result as LoginData;
            if (result.Errors.Count > 0 || session == null) return false;

            _sessionId = session.SessionId;
            Mushrooms = session.Mushrooms;
            Gold = session.Gold;
            Silver = session.Silver;

            return true;
        }

        /// <summary>
        ///     Logs out the current session.
        /// </summary>
        /// <returns>The success of the logout as bool.</returns>
        public async Task<bool> LogoutAsync()
        {
            RequestResult result = await new Request(_sessionId, _serverUri, SfAction.Logout).ExecuteAsync();
            if (result.Errors.Count > 1)
            {
                return result.Errors.Any(e => e == "SessionIdExpired");
            }
            if (!(result.Result is bool)) return false;
            return (bool) result.Result;
        }

        /// <summary>
        ///     Represents the Character Screen Action.
        /// </summary>
        /// <returns>The character of the currently logged in account.</returns>
        public async Task<ICharacter> CharacterAsync()
        {
            var request = new Request(_sessionId, _serverUri, SfAction.Character);
            RequestResult result = await request.ExecuteAsync();
            return result.Result as ICharacter;
        }

        /// <summary>
        ///     Requests a Character via a given predicate.
        /// </summary>
        /// <param name="username">The username to search.</param>
        /// <returns>Null if the name cannot be found.</returns>
        public async Task<ICharacter> RequestCharacterAsync(string username)
        {
            var request = new Request(_sessionId, _serverUri, SfAction.RequestCharacter, new[] {username});
            RequestResult result = await request.ExecuteAsync();
            return result.Result as ICharacter;
        }

        /// <summary>
        ///     Represents the Hall Of Fame Screen Action.
        /// </summary>
        /// <param name="forceLoad">Indicates whether the details of the characters shall be loaded.</param>
        /// <returns>A list of ICharacter.</returns>
        public async Task<ICharacter> HallOfFameAsync(bool forceLoad = false)
        {
            var request = new Request(_sessionId, _serverUri, SfAction.HallOfFame);
            RequestResult result = await request.ExecuteAsync();
            return result.Result as ICharacter;
        }
    }
}