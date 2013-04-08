using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using SfSdk.Data;

namespace SfSdk
{
    internal class SfRequest
    {
        private readonly Uri _serverUri;
        private static readonly Random Random;
        private readonly Uri _requestUri;

        static SfRequest()
        {
            Random = new Random(DateTime.Now.Millisecond);
        }

        internal SfRequest(string sessionId, Uri serverUri, SfAction sfAction, IEnumerable<string> args = null)
        {
            if (serverUri == null) throw new ArgumentNullException("serverUri");
            if (sessionId == null) throw new ArgumentNullException("sessionId");

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

        internal async Task<SfRequestResult> ExecuteAsync()
        {
            try
            {
                WebRequest webRequest = CreateWebRequest();
                using (WebResponse response = await webRequest.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                    return ParseResponseString(reader.ReadToEnd());
            }
            catch (WebException)
            {
                throw new NotImplementedException();
            }
        }

        private WebRequest CreateWebRequest()
        {
            // todo refactor properly
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

        private SfRequestResult ParseResponseString(string responseString)
        {
            if (responseString.StartsWith("+"))
                responseString = responseString.Substring(1);

            if (responseString.StartsWith("E"))
            {
                var fail = (SfFail) int.Parse(responseString.Substring(1, 3));
                string[] failArgs = responseString.Substring(4).Split(';');
                return ProcessFail(fail, failArgs);
            }

            var success = (SfSuccess) int.Parse(responseString.Substring(0, 3));
            string[] successArgs = responseString.Substring(3).Split(';');
            return ProcessSuccess(success, successArgs);
        }

        private SfRequestResult ProcessSuccess(SfSuccess success, string[] args)
        {
            string[] savegameParts;
            var r = new SfRequestResult();
            switch (success)
            {
                case SfSuccess.LoginSuccess:
                case SfSuccess.LoginSuccessBought:
                    if (args.Length < 3) throw new NotImplementedException();
                    string sessionId = args[2];
                    savegameParts = ("0/" + args[0]).Split('/');
                    r.Result = new LoginData(sessionId, savegameParts);
                    break;
                case SfSuccess.LogoutSuccess:
                    r.Result = true;
                    break;
                case SfSuccess.Character:
                case SfSuccess.SearchedPlayerFound:
                    savegameParts = ("0/" + args[0]).Split('/');
                    string guild = args[2];
                    string comment = args[1];
                    r.Result = new Character(savegameParts, guild, comment);
                    break;
                case SfSuccess.HallOfFame:
                    string searchString = null;
                    if (args.Length > 1)
                        searchString = args[1];
                    var tmp = args[0].Split('/');
                    break;
                case SfSuccess.SearchedPlayerNotFound:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(string.Format("Success: {0}", success));
            }
            return r;
        }

        private SfRequestResult ProcessFail(SfFail fail, string[] args)
        {
            var result = new SfRequestResult();
            switch (fail)
            {
                case SfFail.LoginFailed:
                case SfFail.SessionIdExpired:
                case SfFail.ServerDown:
                    result.Errors.Add(fail.ToString());
                    break;
                default:
                    throw new NotImplementedException(fail.ToString());
            }
            return result;
        }
    }
}