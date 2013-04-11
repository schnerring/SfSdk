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

        internal static async Task<IEnumerable<Server>> CreateServersAsync(Country country, bool forceRefresh = false)
        {
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
            string url = Regex.Match(match.Value, ServerUrlPattern).Value;
            url = url.Remove(0, "value=\"".Length);
            url = url.Remove(url.Length - 1, "\"".Length);

            string name = Regex.Match(match.Value, ServerNamePattern).Value;
            name = name.Remove(0, ">".Length);
            name = name.Remove(name.Length - 1, "<".Length);

            var uri = new UriBuilder(url).Uri;

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