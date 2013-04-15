using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SfSdk.Contracts;

namespace SfSdk.Data
{
    /// <summary>
    ///     Implements the functionality of creating a new <see cref="IServer" />.
    /// </summary>
    [Serializable]
    internal class Server : IServer
    {
        // TODO HtmlAgilityPack
        private const string TagPattern = "<option value=\"(.*?)\">(.*?)</option>";
        private const string ServerUrlPattern = "value=\"(.*?)\"";
        private const string ServerNamePattern = ">(.*?)<";
        private static readonly Dictionary<Uri, string> Responses = new Dictionary<Uri, string>();

        private Server(string name, Uri uri)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (uri == null) throw new ArgumentNullException("uri");
            Name = name;
            Uri = uri;
        }

        public string Name { get; private set; }
        public Uri Uri { get; private set; }

        /// <summary>
        ///     Creates an <see cref="ICountry" />.
        /// </summary>
        /// <param name="country">The country where the server belongs to.</param>
        /// <param name="forceRefresh">Indicates whether the server's details shall be re-requested or the cached results shall be returned.</param>
        /// <returns>A <see cref="IEnumerable{T}" /> where T: <see cref="IServer" />.</returns>
        public static async Task<IEnumerable<IServer>> CreateServersAsync(ICountry country, bool forceRefresh = false)
        {
            if (country == null) throw new ArgumentNullException("country");
            if (country == null) throw new ArgumentException("The country's URI must not be null.", "country");

            if (forceRefresh)
                if (Responses.ContainsKey(country.Uri))
                    Responses.Remove(country.Uri);

            if (!Responses.ContainsKey(country.Uri))
                Responses.Add(country.Uri, await new WebClient().DownloadStringTaskAsync(country.Uri));

            return
                Regex.Matches(Responses[country.Uri], TagPattern, RegexOptions.Singleline)
                     .Cast<Match>()
                     .Select(CreateServerFromMatch);
        }

        private static Server CreateServerFromMatch(Match match)
        {
            // TODO catch exceptions?
            string url = Regex.Match(match.Value, ServerUrlPattern).Value;
            url = url.Remove(0, "value=\"".Length);
            url = url.Remove(url.Length - 1, "\"".Length);

            string name = Regex.Match(match.Value, ServerNamePattern).Value;
            name = name.Remove(0, ">".Length);
            name = name.Remove(name.Length - 1, "<".Length);

            Uri uri = new UriBuilder(url).Uri;

            if (string.IsNullOrEmpty(uri.ToString())) name = uri.ToString();

            return new Server(name, uri);
        }

        #region Serialization

        // TODO Implement serialization properly

        protected Server(SerializationInfo info, StreamingContext context)
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