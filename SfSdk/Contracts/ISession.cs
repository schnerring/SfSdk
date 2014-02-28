using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SfSdk.Contracts
{
    /// <summary>
    ///     A host of actions, which can be performed as you log the session in with valid user credentials.
    /// </summary>
    public interface ISession
    {
        /// <summary>
        ///     The mushrooms count the currently logged in session.
        /// </summary>
        int Mushrooms { get; }

        /// <summary>
        ///     The gold count the currently logged in session.
        /// </summary>
        int Gold { get; }

        /// <summary>
        ///     The silver count the currently logged in session.
        /// </summary>
        int Silver { get; }

        /// <summary>
        ///     Logs the current session in.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="md5PasswordHash">The MD5 hash of the password.</param>
        /// <param name="serverUri">The <see cref="Uri"/> of the server to be logged on.</param>
        /// <returns>The success of the login process as <see cref="bool"/>.</returns>
        Task<bool> LoginAsync(string username, string md5PasswordHash, Uri serverUri);

        /// <summary>
        ///     Logs the current session out.
        /// </summary>
        /// <returns>The success of the logout as <see cref="bool"/>.</returns>
        Task<bool> LogoutAsync();

        /// <summary>
        ///     Represents the Character Screen Action.
        /// </summary>
        /// <returns>The <see cref="ICharacter"/> of the currently logged in account.</returns>
        Task<ICharacter> MyCharacterAsync();

        /// <summary>
        ///     Requests a Character via a given predicate.
        /// </summary>
        /// <param name="username">The username to search.</param>
        /// <returns>The <see cref="ICharacter"/> if the name could be found, null if not.</returns>
        Task<ICharacter> RequestCharacterAsync(string username);

        /// <summary>
        ///     Represents the Hall Of Fame Screen Action.
        /// </summary>
        /// <param name="searchString">Search strings may contain the rank or the name of a to be searched character.</param>
        /// <param name="forceLoad">Indicates whether the details of the characters shall be loaded.</param>
        /// <returns>A <see cref="List{T}"/> where T: <see cref="ICharacter"/>/>.</returns>
        Task<IEnumerable<ICharacter>> HallOfFameAsync(string searchString = null, bool forceLoad = false);

        /// <summary>
        ///     Represents the Request Scrapbook Action.
        /// </summary>
        /// <returns>The Scrapbook of a character.</returns>
        Task<object> ScrapbookAsync();
    }
}