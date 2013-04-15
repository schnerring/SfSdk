using System.Threading.Tasks;
using SfSdk.Request;
using SfSdk.Response;

namespace SfSdk.DataSource
{
    /// <summary>
    ///     Used to request data from a S&amp;F data source.
    /// </summary>
    internal interface IRequestSource
    {
        /// <summary>
        ///     Request the data asynchronously.
        /// </summary>
        /// <returns>The response as a <see cref="SfResponse"/>.</returns>
        Task<SfResponse> RequestAsync();
    }
}