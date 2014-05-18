// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguageResourceProvider.cs" company="">
//   Copyright (c) 2014 ebeeb
// </copyright>
// <summary>
//   A service to receive S&F language resources.
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
    ///     A service to receive S&amp;F language resources.
    /// </summary>
    public class LanguageResourceProvider
    {
        /// <summary>
        ///     Caches the data which is downloaded.
        /// </summary>
        private static readonly Dictionary<string, string> ResourceDataDict = new Dictionary<string, string>();

        /// <summary>
        ///     Downloads the english language resources.
        /// </summary>
        /// <returns>
        ///     A dictionary containing the language resources.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     When no network connection is available
        /// </exception>
        public async Task<Dictionary<int, string>> GetLanguageResourcesAsnyc(Uri serverUri)
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
                        ResourceDataDict[serverUrl] = await wc.DownloadStringTaskAsync("http://s1.sfgame.us/lang/sfgame_en.txt");
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
        ///     Downloads the english language resources.
        /// </summary>
        /// <returns>
        ///     A dictionary containing the language resources.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     When no network connection is available
        /// </exception>
        public Dictionary<int, string> GetLanguageResources(Uri serverUri)
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
                        ResourceDataDict[serverUrl] = wc.DownloadString("http://s1.sfgame.us/lang/sfgame_en.txt");
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
            return
                stringData.Split('\n')
                    .Select(line => line.Split('\t'))
                    .Where(lineParts => lineParts.Length == 2)
                    .ToDictionary(lineParts => int.Parse(lineParts[0]), lineParts => lineParts[1]);
        }
    }
}