using System;
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

        public int Mushrooms { get; private set; }
        public int Gold { get; private set; }
        public int Silver { get; private set; }

        public async Task<bool> LoginAsync(string username, string md5PasswordHash, Uri serverUri)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (md5PasswordHash == null) throw new ArgumentNullException("md5PasswordHash");
            if (serverUri == null) throw new ArgumentNullException("serverUri");

            _serverUri = serverUri;
            var request = new SfRequest(EmptySessionId, _serverUri, SF.ActLogin,
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

        public async Task<bool> LogoutAsync()
        {
            var result = await new SfRequest(_sessionId, _serverUri, SF.ActLogout).ExecuteAsync();
            if (result.Errors.Count > 1)
            {
                return result.Errors.Any(e => e == "SessionIdExpired");
            }
            var response = result.Response as LogoutResponse;
            if (response == null) return false;
            return true;
        }

        public async Task<ICharacter> CharacterScreenAsync()
        {
            var request = new SfRequest(_sessionId, _serverUri, SF.ActScreenChar);
            var result = await request.ExecuteAsync();
            return new Character(result.Response as ICharacterResponse);
        }

        public async Task<ICharacter> RequestCharacterAsync(string username)
        {
            var request = new SfRequest(_sessionId, _serverUri, SF.ActRequestChar, new[] { username });
            var result = await request.ExecuteAsync();
            return new Character(result.Response as ICharacterResponse);
        }

        public async Task<ICharacter> HallOfFameAsync(bool forceLoad = false)
        {
            var request = new SfRequest(_sessionId, _serverUri, SF.ActScreenEhrenhalle);
            var result = await request.ExecuteAsync();
            return new Character(result.Response as ICharacterResponse);
        }
    }
}