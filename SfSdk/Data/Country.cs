using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using SfSdk.Contracts;

namespace SfSdk.Data
{
    /// <summary>
    ///     Implements the functionality of creating a new <see cref="ICountry"/>.
    /// </summary>
    [Serializable]
    internal class Country : ICountry
    {
        private Country(string name, Uri uri)
        {
            Name = name;
            Uri = uri;
        }

        public string Name { get; private set; }
        public Uri Uri { get; private set; }
        public IList<IServer> Servers { get; private set; }

        /// <summary>
        ///     Creates an <see cref="ICountry"/>.
        /// </summary>
        /// <param name="name">The country's name.</param>
        /// <param name="uri">The country's <see cref="Uri"/></param>
        /// <param name="forceRefresh">Indicates whether the country's details shall be re-requested or the cached results shall be returned.</param>
        /// <returns>A <see cref="ICountry"/>.</returns>
        public static async Task<ICountry> CreateAsync(string name, Uri uri, bool forceRefresh = false)
        {
            if (uri == null) throw new ArgumentNullException("uri");

            var country = new Country(name ?? uri.ToString(), uri);
            country.Servers = (await Server.CreateServersAsync(country, forceRefresh)).ToList();
            return country;
        }

        #region Serialization

        // TODO Implement serialization properly

        protected Country(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("Name");
            Uri = (Uri) info.GetValue("Uri", typeof (Uri));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Uri", Uri);
        }

        #endregion
    }
}