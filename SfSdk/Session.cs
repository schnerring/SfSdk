using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SfSdk.Constants;
using SfSdk.Contracts;
using SfSdk.Data;
using SfSdk.Request;
using SfSdk.Response;

namespace SfSdk
{
    /// <summary>
    ///     A host of actions, which can be performed as you log the session in with valid user credentials. 
    /// </summary>
    public class Session : ISession
    {
        private const string EmptySessionId = "00000000000000000000000000000000";

        private Uri _serverUri;
        private string _sessionId;
        private IRequestSource _source;

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
        public async Task<bool> LoginAsync(string username, string md5PasswordHash, Uri serverUri)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (md5PasswordHash == null) throw new ArgumentNullException("md5PasswordHash");
            if (serverUri == null) throw new ArgumentNullException("serverUri");

            _serverUri = serverUri;
            _source = new SnFRequestSource(_serverUri);
            var result =
                await
                new SfRequest().ExecuteAsync(_source, EmptySessionId, SF.ActLogin,
                                             new[] {username, md5PasswordHash, "v1.70&random=%2"});

            var response = result.Response as LoginResponse;
            if (result.Errors.Count > 0 || response == null) return false;

            _sessionId = response.SessionId;
            Mushrooms = response.Mushrooms;
            Gold = response.Gold;
            Silver = response.Silver;

            return true;
        }

        /// <summary>
        ///     Logs the current session out.
        /// </summary>
        /// <returns>The success of the logout as <see cref="bool"/>.</returns>
        public async Task<bool> LogoutAsync()
        {
            var result = await new SfRequest().ExecuteAsync(_source, _sessionId, SF.ActLogout);
            if (result.Errors.Count > 1)
            {
                return result.Errors.Any(e => e == "SessionIdExpired");
            }
            var response = result.Response as LogoutResponse;
            if (response == null) return false;
            return true;
        }

        /// <summary>
        ///     Represents the Character Screen Action.
        /// </summary>
        /// <returns>The <see cref="ICharacter"/> of the currently logged in account.</returns>
        public async Task<ICharacter> CharacterScreenAsync()
        {
            var result = await new SfRequest().ExecuteAsync(_source, _sessionId, SF.ActScreenChar);
            return new Character(result.Response as ICharacterResponse);
        }

        /// <summary>
        ///     Requests a Character via a given predicate.
        /// </summary>
        /// <param name="username">The username to search.</param>
        /// <returns>The <see cref="ICharacter"/> if the name could be found, null if not.</returns>
        public async Task<ICharacter> RequestCharacterAsync(string username)
        {
            var result = await new SfRequest().ExecuteAsync(_source, _sessionId, SF.ActRequestChar, new[] {username});
            return new Character(result.Response as ICharacterResponse);
        }

        /// <summary>
        ///     Represents the Hall Of Fame Screen Action.
        /// </summary>
        /// <param name="forceLoad">Indicates whether the details of the characters shall be loaded.</param>
        /// <returns>A <see cref="List{T}"/> where T: <see cref="ICharacter"/>/>.</returns>
        public async Task<ICharacter> HallOfFameAsync(bool forceLoad = false)
        {
            var result = await new SfRequest().ExecuteAsync(_source, _sessionId, SF.ActScreenEhrenhalle);
            return new Character(result.Response as ICharacterResponse);
        }
    }
}