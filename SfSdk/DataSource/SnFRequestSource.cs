using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using SfSdk.Request;
using SfSdk.Response;

namespace SfSdk.DataSource
{
    /// <summary>
    ///     Used to request data from the real S&amp;F servers.
    /// </summary>
    internal class SnFRequestSource : RequestSourceBase
    {
        /// <summary>
        ///     Creates a new instance of SnFRequestSource.
        /// </summary>
        /// <param name="uriFactory">
        ///     The <see cref="SnFUriFactory" /> containing information about the request <see cref="Uri" />'s parts.
        /// </param>
        public SnFRequestSource(SnFUriFactory uriFactory) : base(uriFactory)
        {
        }

        private WebRequest CreateWebRequest(SnFUriFactory uriFactory)
        {
            // TODO refactor properly, very ugly ATM => static Cookie
            var webRequest = (HttpWebRequest) WebRequest.Create(uriFactory.RequestUri);

            webRequest.Host = uriFactory.ServerUri.Host;
            webRequest.KeepAlive = true;
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.43 Safari/537.31";
            webRequest.Accept = "*/*";
            webRequest.Referer = uriFactory.ServerUri.AbsoluteUri;
//            webRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
            webRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "de-DE,de;q=0.8,en-US;q=0.6,en;q=0.4");
            webRequest.Headers.Add(HttpRequestHeader.AcceptCharset, "ISO-8859-1,utf-8;q=0.7,*;q=0.3");
            webRequest.Headers.Add(HttpRequestHeader.Cookie, "904abc7e0bd65dd5396d8696ae2446e8=1");

            return webRequest;
        }

        public override async Task<SfResponse> RequestAsync()
        {
            WebRequest webRequest = CreateWebRequest((SnFUriFactory) UriFactory);
            try
            {
                using (WebResponse response = await webRequest.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream == null) throw new NotImplementedException();
                    using (var reader = new StreamReader(stream)) // TODO
                        return new SfResponse(reader.ReadToEnd());
                }
            }
            catch (WebException)
            {
                throw new NotImplementedException("Network connection lost.");
            }
        }
    }
}