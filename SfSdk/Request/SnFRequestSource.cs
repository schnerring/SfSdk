using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using SfSdk.Constants;
using SfSdk.Logging;
using SfSdk.Response;

namespace SfSdk.Request
{
    /// <summary>
    ///     Used to request data from the real S&amp;F servers.
    /// </summary>
    internal class SnFRequestSource : IRequestSource
    {
        private readonly Uri _serverUri;
        private static readonly ILog Log = LogManager.GetLog(typeof (SnFRequestSource));

        /// <summary>
        ///     Creates a new instance of SnFRequestSource.
        /// </summary>
        /// <param name="serverUri">The server <see cref="Uri" /> which the request source is going to request.</param>
        public SnFRequestSource(Uri serverUri)
        {
            _serverUri = serverUri;
        }

        public async Task<ISfResponse> RequestAsync(string sessionId, SF action, IEnumerable<string> args = null)
        {
            var uriWrapper = new SnFUriWrapper(sessionId, _serverUri, action, args);

            Log.Info("SID:    {0}", uriWrapper.SessionId);
            Log.Info("Action: {0}", uriWrapper.Action);
            Log.Info("Args:   {0}", uriWrapper.Args ?? "null");
            Log.Info("URL:    {0}", uriWrapper.RequestUri.AbsoluteUri);

            WebRequest webRequest = CreateWebRequest(uriWrapper);
            try
            {
                using (WebResponse response = await webRequest.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream == null) throw new NotImplementedException();
                    using (var reader = new StreamReader(stream))
                        return new SfResponse(reader.ReadToEnd());
                }
            }
            catch (WebException)
            {
                throw new NotImplementedException("Network connection lost.");
            }
        }

        private static WebRequest CreateWebRequest(IUriWrapper uriWrapper)
        {
            // TODO refactor properly, very ugly ATM => static Cookie
            var webRequest = (HttpWebRequest)WebRequest.Create(uriWrapper.RequestUri);

            webRequest.Host = uriWrapper.ServerUri.Host;
            webRequest.KeepAlive = true;
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.43 Safari/537.31";
            webRequest.Accept = "*/*";
            webRequest.Referer = uriWrapper.ServerUri.AbsoluteUri;
//            webRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
            webRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "de-DE,de;q=0.8,en-US;q=0.6,en;q=0.4");
            webRequest.Headers.Add(HttpRequestHeader.AcceptCharset, "ISO-8859-1,utf-8;q=0.7,*;q=0.3");
            webRequest.Headers.Add(HttpRequestHeader.Cookie, "904abc7e0bd65dd5396d8696ae2446e8=1");

            return webRequest;
        }
    }
}