using CodeArt.MatomoTracking.Attributes;
using CodeArt.MatomoTracking.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;


namespace CodeArt.MatomoTracking
{
    public class MatomoTracker : IMatomoTracker
    {
        private readonly ILogger _logger = null;
        private readonly string _matomoUrl;
        private readonly string _siteId;
        private readonly Random _rand;
        public MatomoTracker(IServiceProvider services, IOptions<MatomoOptions> options)
        {
            _logger = services.GetService<ILogger<MatomoTracker>>();
            _matomoUrl = options.Value.MatomoUrl;
            _siteId = options.Value.SiteId;
            _rand = new Random();
        }



        //https://developer.matomo.org/api-reference/tracking-api

        private Uri BuildUri(Action<NameValueCollection> SetParams)
        {
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
            _logger?.LogInformation("Tracking url built: " + url.ToString());
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogError($"Matomo tracking failed: Status code: {response.StatusCode} and reason: {response.ReasonPhrase}");
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
            if (item.UserTime != null && item.UserTime.HasValue)
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
