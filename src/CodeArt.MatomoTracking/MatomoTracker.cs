using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CodeArt.MatomoTracking
{
    public class MatomoTracker
    {
        private readonly string _matomoUrl;
        private readonly string _siteId;
        private readonly Random _rand;
        public MatomoTracker(string MatomoUrl, string SiteId)
        {
            //validate input is not null
            if (string.IsNullOrEmpty(MatomoUrl))
                throw new ArgumentNullException("MatomoUrl");
            _matomoUrl = MatomoUrl;
            _siteId = SiteId;
            _rand = new Random();
        }

        //https://developer.matomo.org/api-reference/tracking-api

        private Uri BuildUri(Action<NameValueCollection> SetParams)
        {
            // /matomo.php
            UriBuilder rt = new UriBuilder(_matomoUrl + (_matomoUrl.EndsWith("/") ? "" : "/") + "matomo.php?idsite=" + _siteId + "&rec=1");
            var query = HttpUtility.ParseQueryString(rt.Query);
            query["idsite"] = _siteId;
            query["rec"] = "1";
            query["rand"] = _rand.Next(100000000).ToString();
            query["apiv"] = "1";
            SetParams(query);
            rt.Query = query.ToString();
            return rt.Uri;
        }


        public async Task Track(Action<NameValueCollection> SetParams)
        {
            var url = BuildUri(SetParams);
            var client = new HttpClient();
            Console.WriteLine(url); //Temp
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                //TODO: check response and handle errors
                Console.WriteLine("Something went wrong: " + response.ReasonPhrase);
            }
        }

        public string GeneratePageViewId()
        {
            const string chars = "abcdefghijklmnopqrstuvxyz0123456789";
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[_rand.Next(s.Length)]).ToArray());
        }
    }
}
