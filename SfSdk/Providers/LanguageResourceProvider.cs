using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using SfSdk.Constants;

namespace SfSdk.Providers
{
    /// <summary>
    ///     A service to receive S&amp;F language resources.
    /// </summary>
    internal class LanguageResourceProvider : ResourceProviderBase
    {
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);
        
        /// <summary>
        ///     Downloads the english language resources.
        /// </summary>
        /// <returns>
        ///     A dictionary containing the language resources.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     When no network connection is available
        /// </exception>
        public override Dictionary<SF, string> GetResources(Uri serverUri)
        {
            if (serverUri == null) throw new ArgumentNullException("serverUri");

            var uri = new Uri(serverUri, "lang/sfgame_en.txt");
            var url = uri.ToString();

            Func<string> fillResourceDict = () =>
            {
                try
                {
                    using (var wc = new WebClient())
                    {
                        var randomizedUri = new Uri(uri,
                            "?rnd=" + Random.NextDouble().ToString("N" + 16, CultureInfo.GetCultureInfo("en-US")));
                        return wc.DownloadString(randomizedUri);
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