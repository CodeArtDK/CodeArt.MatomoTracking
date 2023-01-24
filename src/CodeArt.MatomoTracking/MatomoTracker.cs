using CodeArt.MatomoTracking.Attributes;
using CodeArt.MatomoTracking.Interfaces;
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
            var response = await client.GetAsync(url); //TODO: Also support POST if GET is too long
            if (!response.IsSuccessStatusCode)
            {
                //TODO: check response and handle errors
                Console.WriteLine("Something went wrong: " + response.ReasonPhrase);
            }
        }


        protected void SetParams(NameValueCollection query, ITrackingItem item)
        {
            var props = item.GetType().GetProperties();
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttributes(typeof(QueryParameterAttribute), true).FirstOrDefault() as QueryParameterAttribute;
                if (attr != null)
                {
                    var value = prop.GetValue(item);
                    if (value != null)
                    {
                        if (value is bool)
                        {
                            query[attr.Name] = (bool)value ? "1" : "0";
                        }
                        else
                        {
                            query[attr.Name] = value.ToString();
                        }
                    }
                }
            }
            if(item.UserTime!=null && item.UserTime.HasValue)
            {
                {
                    query["h"] = item.UserTime.Value.Hours.ToString();
                    query["m"] = item.UserTime.Value.Minutes.ToString();
                    query["s"] = item.UserTime.Value.Seconds.ToString();
                }
            }
            if (item.Dimensions != null && item.Dimensions.Count > 0)
            {
                foreach (var dim in item.Dimensions)
                {
                    query["dimension" + dim.Key] = dim.Value;
                }
            }
        }

        public async Task Track(ITrackingItem trackingItem)
        {
            await Track(query => SetParams(query, trackingItem));
        }

        public string GeneratePageViewId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[_rand.Next(s.Length)]).ToArray());
        }
    }
}
