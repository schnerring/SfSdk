using System.Threading.Tasks;

namespace SfSdk
{
    /// <summary>
    ///     Used to request data from a S&amp;F data source.
    /// </summary>
    internal interface IRequestSource
    {
        /// <summary>
        ///     Request the data asynchronously.
        /// </summary>
        /// <returns>The response as a <see cref="RequestResult"/>.</returns>
        Task<RequestResult> RequestAsync();
    }
}