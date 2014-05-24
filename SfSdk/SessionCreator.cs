using SfSdk.Contracts;
using SfSdk.Request;

namespace SfSdk
{
    /// <summary>
    ///     A wrapper which helps creating new <see cref="Session"/> instances.
    /// </summary>
    public static class SessionCreator
    {
        /// <summary>
        ///     Creates a new <see cref="Session"/> instance which queries the default <see cref="SnFRequestSource"/>.
        /// </summary>
        /// <returns>The new <see cref="Session"/> as <see cref="ISession"/>.</returns>
        public static ISession Create()
        {
            return new Session();
        }
    }
}
