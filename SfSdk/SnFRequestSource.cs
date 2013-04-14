using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SfSdk
{
    /// <summary>
    ///     Used to request data from the real S&amp;F servers.
    /// </summary>
    internal class SnFRequestSource : IRequestSource
    {
        private readonly Uri _requestUri;
        private readonly Uri _serverUri;
        private readonly WebRequest _webRequest;

        /// <summary>
        ///     Creates a new instance of SnFRequestSource.
        /// </summary>
        /// <param name="serverUri">The server <see cref="Uri"/> of the server to be requested.</param>
        /// <param name="requestUri">The actual <see cref="Uri"/> containing all the parameters to request a S&amp;F server properly.</param>
        public SnFRequestSource(Uri serverUri, Uri requestUri)
        {
            if (serverUri == null) throw new ArgumentNullException("serverUri");
            if (requestUri == null) throw new ArgumentNullException("requestUri");

            _serverUri = serverUri;
            _requestUri = requestUri;
            _webRequest = CreateWebRequest();
        }

        /// <summary>
        ///     Request the data asynchronously.
        /// </summary>
        /// <returns>The response as a RequestResult.</returns>
        public async Task<RequestResult> RequestAsync()
        {
            using (WebResponse response = await _webRequest.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream)) // TODO
                return new RequestResult(reader.ReadToEnd());
        }

        private WebRequest CreateWebRequest()
        {
            // TODO refactor properly, very ugly ATM => static Cookie
            var webRequest = (HttpWebRequest) WebRequest.Create(_requestUri);

            webRequest.Host = _serverUri.Host;
            webRequest.KeepAlive = true;
            webRequest.UserAgent =
                "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.43 Safari/537.31";
            webRequest.Accept = "*/*";
            webRequest.Referer = _serverUri.AbsoluteUri;
            //            webRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
            webRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "de-DE,de;q=0.8,en-US;q=0.6,en;q=0.4");
            webRequest.Headers.Add(HttpRequestHeader.AcceptCharset, "ISO-8859-1,utf-8;q=0.7,*;q=0.3");
            webRequest.Headers.Add(HttpRequestHeader.Cookie, "904abc7e0bd65dd5396d8696ae2446e8=1");

            return webRequest;
        }
    }
}