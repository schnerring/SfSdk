using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using SfSdk.Constants;
using SfSdk.Logging;

namespace SfSdk
{
    internal class Request
    {
        private static readonly ILog Log = LogManager.GetLog(typeof (Request));
        private static readonly Random Random;

        private readonly string _action;
        private readonly string _args;
        private readonly Guid _id;
        private readonly Uri _requestUri;
        private readonly Uri _serverUri;
        private readonly string _sessionId;

        static Request()
        {
            Random = new Random(DateTime.Now.Millisecond);
        }

        internal Request(string sessionId, Uri serverUri, SF action, IEnumerable<string> args = null)
        {
            if (sessionId == null) throw new ArgumentNullException("sessionId");
            if (serverUri == null) throw new ArgumentNullException("serverUri");
            if (sessionId.Length != 32) throw new ArgumentException("SessionId must have a length of 32", "sessionId");

            _id = Guid.NewGuid();
            _sessionId = sessionId;
            _serverUri = serverUri;
            _action = ((int) action).ToString(CultureInfo.InvariantCulture);
            while (_action.Length < 3)
                _action = _action.Insert(0, "0");
            if (args != null) _args = string.Join(";", args);
            _requestUri = BuildRequestUri();
        }

        private Uri BuildRequestUri()
        {
            string url = string.Empty;
            url += _serverUri;
            url += "request.php?req=";
            url += _sessionId;
            url += _action;
            url += _args;
            url += "&rnd=";
            url += Random.Next(2000000000);
            url += Math.Round(DateTime.Now.ToUnixTimeStamp());

            return new UriBuilder(url).Uri;
        }

        internal async Task<RequestResult> ExecuteAsync()
        {
            try
            {
                WebRequest webRequest = CreateWebRequest();

                Log.Info("Request started:  ID = {0}", _id);
                Log.Info("    SID:    {0}", _sessionId);
                Log.Info("    Action: {0}", _action);
                Log.Info("    Args:   {0}", _args ?? "null");
                Log.Info("    URL:    {0}", _requestUri.AbsoluteUri);

                RequestResult result = await RequestResult(webRequest);

                Log.Info("Request finished: ID = {0}", _id);
                return result;
            }
            catch (WebException)
            {
                throw new NotImplementedException();
            }
        }

        private static async Task<RequestResult> RequestResult(WebRequest webRequest)
        {
            using (WebResponse response = await webRequest.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream)) // TODO
                    return new RequestResult(reader.ReadToEnd());
            }
        }

        private WebRequest CreateWebRequest()
        {
            // todo refactor properly, very ugly ATM => static Cookie
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