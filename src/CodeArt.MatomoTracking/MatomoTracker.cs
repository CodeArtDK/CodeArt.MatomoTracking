using CodeArt.MatomoTracking.Attributes;
using CodeArt.MatomoTracking.Interfaces;
using CodeArt.MatomoTracking.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;


namespace CodeArt.MatomoTracking
{
    public class MatomoTracker : IMatomoTracker
    {
        private readonly ILogger _logger = null;
        private readonly string _matomoUrl;
        private readonly string _siteId;
        private readonly string _authToken;
        private readonly bool _verbose;
        private readonly bool _trackBots;
        private readonly Random _rand;
        private readonly HttpClient _httpClient;
        
        public MatomoTracker(IServiceProvider services, IOptions<MatomoOptions> options, HttpClient httpClient)
        {
            _logger = services.GetService<ILogger<MatomoTracker>>();
            _httpClient = httpClient;
            _matomoUrl = options.Value.MatomoHostname;
            if (!_matomoUrl.StartsWith("https://"))
            {
                _matomoUrl = "https://" + _matomoUrl;
            }
            if(!_matomoUrl.EndsWith("matomo.php", StringComparison.InvariantCultureIgnoreCase))
            {
                _matomoUrl = _matomoUrl.TrimEnd('/') + "/matomo.php";
            }
            _siteId = options.Value.SiteId;
            _authToken = options.Value.AuthToken;
            _verbose = options.Value.VerboseLogging;
            _trackBots = options.Value.TrackBots;
            _rand = new Random();
        }


        //https://developer.matomo.org/api-reference/tracking-api
        

        private string CreateQueryString(NameValueCollection query,Action<NameValueCollection> SetParams)
        {
            query["idsite"] = _siteId;
            query["rec"] = "1";
            query["rand"] = _rand.Next(100000000).ToString();
            query["apiv"] = "1";
            if (_trackBots)
            {
                query["bots"] = "1";
            }
            if (!string.IsNullOrEmpty(_authToken))
            {
                query["token_auth"] = _authToken;
            }
            //Set additional parameters
            SetParams(query);
            return query.ToString();
        }

        private Uri BuildUri(Action<NameValueCollection> SetParams = null)
        {
            UriBuilder rt = new UriBuilder(_matomoUrl);
            if (SetParams != null)
                rt.Query = CreateQueryString(HttpUtility.ParseQueryString(rt.Query),SetParams);
            return rt.Uri;
        }


        public async Task Track(Action<NameValueCollection> SetParams)
        {
            var url = BuildUri(SetParams);
            if (_verbose)
            {
                _logger?.LogInformation("Tracking url built: " + url.ToString());
            }
            bool usePost = (url.ToString().Length > 2048);
            try
            {
                var response = (usePost) ? await _httpClient.PostAsync(url, null) : await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    _logger?.LogError($"Matomo tracking failed: Status code: {response.StatusCode} and reason: {response.ReasonPhrase}");
                }
                else
                {
                    if (_verbose)
                    {
                        _logger?.LogInformation("Tracked successfully");
                    }
                }
            } catch (Exception exc)
            {
                if (_verbose)
                {
                    _logger?.LogError(exc, $"An error, {exc.Message}, occurred while sending the tracking information to the Matomo server at this url: {url}");
                }
                else {
                    _logger?.LogError(exc, $"An error, {exc.Message}, occurred while sending the tracking information to the Matomo server.");
                }
                
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
                    if (value != null && (!prop.Name.StartsWith("Override") || !string.IsNullOrEmpty(_authToken)))
                    {
                        if (value is bool)
                        {
                            query[attr.Name] = (bool)value ? "1" : "0";
                        }
                        else if(value is DateTime? && (value as DateTime?).HasValue)
                        {
                            DateTimeOffset dto = new DateTimeOffset((value as DateTime?).Value.ToUniversalTime());
                            query[attr.Name] = dto.ToUnixTimeSeconds().ToString();
                        } 
                        else if(value is decimal? && (value as Decimal?).HasValue)
                        {
                            query[attr.Name] = (value as Decimal?).Value.ToString(CultureInfo.InvariantCulture);
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
            if(item is EcommerceTrackingItem)
            {
                var arr=(item as EcommerceTrackingItem).Items.Select(itm => new object[] { itm.SKU, itm.Name, itm.Category, itm.Price, itm.Quantity }).ToList();
                query["ec_items"] = JsonSerializer.Serialize(arr);
            }
        }

        public async Task Track(ITrackingItem trackingItem)
        {
            await Track(query => SetParams(query, trackingItem));
        }

        /// <summary>
        /// Bulk track multiple tracking requests in a single payload
        /// </summary>
        /// <param name="trackingItems">Items to be tracked</param>
        /// <returns>The async task to be executed</returns>
        public async Task Track(params ITrackingItem[] trackingItems)
        {
            var url = BuildUri();

            var payload = new
            {
                requests = trackingItems.Select(item =>
                {
                    var query = HttpUtility.ParseQueryString(string.Empty);
                    return "?" + CreateQueryString(query,q => SetParams(q, item));
                }).ToArray()
            };

            _logger?.LogInformation("Tracking url for bulk tracking built: " + url.ToString());
            _logger?.LogInformation("Bulk tracking payload: " + JsonSerializer.Serialize(payload));
            var response = await _httpClient.PostAsync(url, new StringContent(JsonSerializer.Serialize(payload)));
            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogError($"Matomo tracking failed: Status code: {response.StatusCode} and reason: {response.ReasonPhrase}");
            } else
            {
                _logger?.LogInformation("Tracked successfully");
            }
        }


        private string GenerateID(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_rand.Next(s.Length)]).ToArray());
        }
        
        public string GeneratePageViewId()
        {
            return GenerateID(6);
        }

        public string GenerateVisitorID()
        {
            return GenerateID(16);
        }
    }
}
