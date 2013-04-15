using System;
using System.Threading.Tasks;
using SfSdk.Request;
using SfSdk.Response;

namespace SfSdk.DataSource
{
    /// <summary>
    ///     Used to request data from a S&amp;F data source.
    /// </summary>
    internal abstract class RequestSourceBase : IRequestSource
    {
        private readonly IUriFactory _uriFactory;

        /// <summary>
        ///     Initializes the arguments needed for every S&amp;F data source.
        /// </summary>
        /// <param name="uriFactory">The <see cref="IUriFactory" /> containing information about the request <see cref="Uri"/> parts.</param>
        protected RequestSourceBase(IUriFactory uriFactory)
        {
            if (uriFactory == null) throw new ArgumentNullException("uriFactory");
            _uriFactory = uriFactory;
        }

        /// <summary>
        ///     The <see cref="IUriFactory" />.
        /// </summary>
        public IUriFactory UriFactory
        {
            get { return _uriFactory; }
        }

        public abstract Task<SfResponse> RequestAsync();
    }
}