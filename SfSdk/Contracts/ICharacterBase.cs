using System.Threading.Tasks;

namespace SfSdk.Contracts
{
    /// <summary>
    ///     Contains basic information about a S&amp;F character.
    /// </summary>
    public interface ICharacterBase
    {
        /// <summary>
        ///     The character's username.
        /// </summary>
        string Username { get; }

        /// <summary>
        ///     The character's rank.
        /// </summary>
        int Rank { get; }

        /// <summary>
        ///     The caracter's guild.
        /// </summary>
        string Guild { get; }

        /// <summary>
        ///     The caracter's level.
        /// </summary>
        int Level { get; }

        /// <summary>
        ///     The caracter's honor.
        /// </summary>
        int Honor { get; }

        /// <summary>
        ///     Requests the S&amp;F server to refresh the character's data.
        /// </summary>
        Task Refresh(bool force = true);

    }
}