using System;
using System.Collections.Generic;
using System.Net;
using SfSdk.Constants;

namespace SfSdk.Providers
{
    /// <summary>
    ///     A service to receive S&amp;F configuration resources.
    /// </summary>
    internal class ConfigurationResourceProvider : ResourceProviderBase
    {
        /// <summary>
        ///     Downloads the english configuration resources.
        /// </summary>
        /// <returns>
        ///     A dictionary containing the configuration resources.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     When no network connection is available
        /// </exception>
        public override Dictionary<SF, string> GetResources(Uri serverUri)
        {
            if (serverUri == null) throw new ArgumentNullException("serverUri");

            var uri = new Uri(serverUri, "client_cfg.php");
            var url = uri.ToString();

            Func<string> fillResourceDict = () =>
            {
                try
                {
                    using (var wc = new WebClient())
                    {
                        return wc.DownloadString(uri);
                    }
                }
                catch (WebException)
                {
                    throw new NotImplementedException("Network connection lost.");
                }
            };
            return GetResources(url, fillResourceDict);
        }
    }
}