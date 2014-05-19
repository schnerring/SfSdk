using System;
using System.Collections.Generic;
using SfSdk.Constants;

namespace SfSdk.Providers
{
    internal interface IResourceProvider
    {
        /// <summary>
        ///     Downloads the resources.
        /// </summary>
        /// <returns>
        ///     A dictionary containing the resources.
        /// </returns>
        Dictionary<SF, string> GetResources(Uri serverUri);
    }
}