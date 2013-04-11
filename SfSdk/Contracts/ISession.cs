using System;
using System.Threading.Tasks;

namespace SfSdk.Contracts
{
    public interface ISession
    {
        int Mushrooms { get; }
        int Gold { get; }
        int Silver { get; }

        /// <summary>
        ///     Logs the current session in.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="md5PasswordHash">The MD5 hash of the password.</param>
        /// <param name="serverUri">The Uri of the server to be logged on.</param>
        /// <returns>The success of the login as bool.</returns>
        Task<bool> LoginAsync(string username, string md5PasswordHash, Uri serverUri);

        /// <summary>
        ///     Logs the current session out.
        /// </summary>
        /// <returns>The success of the logout as bool.</returns>
        Task<bool> LogoutAsync();

        /// <summary>
        ///     Represents the Character Screen Action.
        /// </summary>
        /// <returns>The character of the currently logged in account.</returns>
        Task<ICharacter> CharacterAsync();

        /// <summary>
        ///     Requests a Character via a given predicate.
        /// </summary>
        /// <param name="username">The username to search.</param>
        /// <returns>Null if the name cannot be found.</returns>
        Task<ICharacter> RequestCharacterAsync(string username);

        /// <summary>
        ///     Represents the Hall Of Fame Screen Action.
        /// </summary>
        /// <param name="forceLoad">Indicates whether the details of the characters shall be loaded.</param>
        /// <returns>A list of ICharacter.</returns>
        Task<ICharacter> HallOfFameAsync(bool forceLoad = false);
    }
}