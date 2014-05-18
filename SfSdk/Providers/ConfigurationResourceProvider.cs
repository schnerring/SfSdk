// --------------------------------------------------------------------------------------------------------------------
// <copyright file="configurationResourceProvider.cs" company="">
//   Copyright (c) 2014 ebeeb
// </copyright>
// <summary>
//   A service to receive S&F configuration resources.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SfSdk.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    ///     A service to receive S&amp;F configuration resources.
    /// </summary>
    public class ConfigurationResourceProvider
    {
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        ///     Caches the data which is downloaded.
        /// </summary>
        private static readonly Dictionary<string, string> ResourceDataDict = new Dictionary<string, string>();

        /// <summary>
        ///     Downloads the english configuration resources.
        /// </summary>
        /// <returns>
        ///     A dictionary containing the configuration resources.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     When no network connection is available
        /// </exception>
        public async Task<Dictionary<int, string>> GetConfigurationResourcesAsnyc(Uri serverUri)
        {
            if (serverUri == null) throw new ArgumentNullException("serverUri");

            var serverUrl = serverUri.ToString();
            if (!ResourceDataDict.ContainsKey(serverUrl))
                ResourceDataDict.Add(serverUrl, null);

            try
            {
                using (var wc = new WebClient())
                {
                    if (string.IsNullOrWhiteSpace(ResourceDataDict[serverUrl]))
                    {
                        ResourceDataDict[serverUrl] =
                            await
                                wc.DownloadStringTaskAsync(new Uri(serverUri, "client_cfg.php"));
                    }
                }
            }
            catch (WebException)
            {
                throw new NotImplementedException("Network connection lost.");
            }

            return ProcessStringData(ResourceDataDict[serverUrl]);
        }

        /// <summary>
        ///     Downloads the english configuration resources.
        /// </summary>
        /// <returns>
        ///     A dictionary containing the configuration resources.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     When no network connection is available
        /// </exception>
        public Dictionary<int, string> GetConfigurationResources(Uri serverUri)
        {
            if (serverUri == null) throw new ArgumentNullException("serverUri");

            var serverUrl = serverUri.ToString();
            if (!ResourceDataDict.ContainsKey(serverUrl))
                ResourceDataDict.Add(serverUrl, null);

            try
            {
                using (var wc = new WebClient())
                {
                    if (string.IsNullOrWhiteSpace(ResourceDataDict[serverUrl]))
                    {
                        ResourceDataDict[serverUrl] =
                            wc.DownloadString(new Uri(serverUri, "client_cfg.php"));
                    }
                }
            }
            catch (WebException)
            {
                throw new NotImplementedException("Network connection lost.");
            }

            return ProcessStringData(ResourceDataDict[serverUrl]);
        }

        private static Dictionary<int, string> ProcessStringData(string stringData)
        {
            var result = new Dictionary<int, string>();
            var configurationData =
                stringData.Split('\n')
                          .Select(line => line
                          .Split('\t'))
                          .Where(lineParts => lineParts.Length == 2);

            foreach (var line in configurationData)
            {
                var index = int.Parse(line[0]);
                if (result.ContainsKey(index)) continue;
                result.Add(index, line[1]);
            }

            return result;
        }
    }
}