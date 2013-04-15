using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SfSdk.Contracts;
using SfSdk.Data;

namespace SfSdk.Providers
{
    /// <summary>
    ///     A service to receive information about countries where S&amp;F is available.
    /// </summary>
    public class CountryProvider : ICountryProvider
    {
        private static string _response;
        
        public async Task<IEnumerable<ICountry>> GetCountriesAsync(bool forceRefresh = false)
        {
            if (_response == null || forceRefresh)
            {
                await GetResponse();
            }

            var document = new HtmlDocument();
            document.LoadHtml(_response);

            var inputElements = document.DocumentNode.SelectNodes("//input");
            IEnumerable<Task<ICountry>> tasks = inputElements.Where(i => i.Attributes.Any(a => a.Name == "title") &&
                                                                         i.Attributes.Any(a => a.Name == "onclick"))
                                                             .Select(i => CreateCountryAsync(i, forceRefresh));
            return await Task.WhenAll(tasks);
        }

        private static async Task GetResponse()
        {
            // todo refactor WebRequest properly

            var webRequest = (HttpWebRequest) WebRequest.Create(new UriBuilder("http://www.sfgame.de/").Uri);
            
//            webRequest.Host = "s25.sfgame.de";
//            webRequest.KeepAlive = true;
//            webRequest.UserAgent =
//                "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.43 Safari/537.31";
//            webRequest.Accept = "*/*";
//            webRequest.Referer = "http://s25.sfgame.de/";
//            webRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
//            webRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "de-DE,de;q=0.8,en-US;q=0.6,en;q=0.4");
//            webRequest.Headers.Add(HttpRequestHeader.AcceptCharset, "ISO-8859-1,utf-8;q=0.7,*;q=0.3");
//            webRequest.Headers.Add(HttpRequestHeader.Cookie, "904abc7e0bd65dd5396d8696ae2446e8=1");
            try
            {
                using (var responseStream = (HttpWebResponse) await webRequest.GetResponseAsync())
                using (var streamReader = new StreamReader(responseStream.GetResponseStream()))
                    _response = await streamReader.ReadToEndAsync();
            }
            catch (WebException)
            {
                throw new NotImplementedException();
            }
        }

        private static async Task<ICountry> CreateCountryAsync(HtmlNode node, bool forceRefresh)
        {
            // TODO catch exceptions?
            var name = node.Attributes.Where(a => a.Name == "title").Select(a => a.Value).FirstOrDefault();
            var url = node.Attributes.Where(a => a.Name == "onclick").Select(a => a.Value).FirstOrDefault();
            Uri uri = null;
            if (url != null)
            {
                url = url.Substring(url.IndexOf('\'') + 1, url.LastIndexOf('\'') - url.IndexOf('\'') - 1);
                uri = new UriBuilder(url).Uri;
            }
            return await Country.CreateAsync(name, uri, forceRefresh);
        }
    }
}