using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using SfSdk.Enums;

namespace SfSdk
{
    internal class Request
    {
        private readonly Uri _serverUri;
        private static readonly Random Random;
        private readonly Uri _requestUri;

        static Request()
        {
            Random = new Random(DateTime.Now.Millisecond);
        }

        internal Request(string sessionId, Uri serverUri, SfAction sfAction, IEnumerable<string> args = null)
        {
            if (sessionId == null) throw new ArgumentNullException("sessionId");
            if (serverUri == null) throw new ArgumentNullException("serverUri");
            if (sessionId.Length != 32) throw new ArgumentException("SessionId must have a length of 32", "sessionId");

            _serverUri = serverUri;

            string joinedArgs = null;
            if (args != null) joinedArgs = string.Join(";", args);

            string actionString = ((int) sfAction).ToString(CultureInfo.InvariantCulture);
            while (actionString.Length < 3)
                actionString = actionString.Insert(0, "0");

            string url = string.Empty;
                   url += serverUri;
                   url += "request.php?req=";
                   url += sessionId;
                   url += actionString;
                   url += joinedArgs;
                   url += "&rnd=";
                   url += Random.Next(2000000000);
                   url += Math.Round(DateTime.Now.ToUnixTimeStamp());

            _requestUri = new UriBuilder(url).Uri;
        }

        internal async Task<RequestResult> ExecuteAsync()
        {
            try
            {
                WebRequest webRequest = CreateWebRequest();
                using (WebResponse response = await webRequest.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                    return new RequestResult(reader.ReadToEnd());
            }
            catch (WebException)
            {
                throw new NotImplementedException();
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