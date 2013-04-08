using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using SfSdk.Contracts;

namespace SfSdk.Data
{
    [Serializable]
    internal class Country : ICountry
    {
        private Country(string name, Uri uri)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (uri == null) throw new ArgumentNullException("uri");
            Name = name;
            Uri = uri;
        }

        public string Name { get; private set; }
        public Uri Uri { get; private set; }
        public IList<IServer> Servers { get; private set; }

        internal static async Task<Country> CreateAsync(string name, Uri uri, bool forceRefresh = false)
        {
            var country = new Country(name, uri);
            country.Servers = (await Server.CreateServersAsync(country, forceRefresh)).ToList<IServer>();
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