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
        private static string resourceData;

        /// <summary>
        ///     Downloads the english language resources.
        /// </summary>
        /// <returns>
        ///     A dictionary containing the language resources.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     When no network connection is available
        /// </exception>
        public async Task<Dictionary<int, string>> GetLanguageResourcesAsnyc()
        {
            try
            {
                using (var wc = new WebClient())
                {
                    if (string.IsNullOrWhiteSpace(resourceData))
                    {
                        resourceData = await wc.DownloadStringTaskAsync("http://s1.sfgame.us/lang/sfgame_en.txt");
                    }
                }
            }
            catch (WebException)
            {
                throw new NotImplementedException("Network connection lost.");
            }

            return
                resourceData.Split('\n')
                            .Select(line => line.Split('\t'))
                            .Where(lineParts => lineParts.Length == 2)
                            .ToDictionary(lineParts => int.Parse(lineParts[0]), lineParts => lineParts[1]);
        }
    }
}