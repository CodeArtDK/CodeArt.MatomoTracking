using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Web;

namespace CodeArt.MatomoTracking
{
    public class MatomoReporting : IMatomoReporting
    {
        private readonly ILogger _logger = null;
        private readonly string _matomoUrl;
        private readonly string _siteId;
        private readonly string _authToken;
        private readonly HttpClient _httpClient;

        public MatomoReporting(IServiceProvider services, IOptions<MatomoOptions> options, HttpClient httpClient)
        {
            _logger = services.GetService<ILogger<MatomoReporting>>();
            _httpClient = httpClient;
            _matomoUrl = options.Value.MatomoHostname;
            _authToken = options.Value.AuthToken;
            if (_matomoUrl.EndsWith("matomo.php", StringComparison.InvariantCultureIgnoreCase))
            {
                //End with "/index.php" instead of "/matomo.php"
                _matomoUrl = _matomoUrl.Substring(0, _matomoUrl.Length - 10) + "/index.php";
            }
            else if (_matomoUrl.EndsWith('/'))
            {
                _matomoUrl = _matomoUrl + "index.php";
            }
            else
            {
                _matomoUrl = _matomoUrl + "/index.php";
            }
            _siteId = options.Value.SiteId;
        }

        //Segmentation
        //https://developer.matomo.org/api-reference/reporting-api

        private string CreateQueryString(NameValueCollection query, Action<NameValueCollection> SetParams)
        {
            query["module"] = "API";
            query["idSite"] = _siteId;
            query["format"] = "json";
            if (_authToken != null)
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
                rt.Query = CreateQueryString(HttpUtility.ParseQueryString(rt.Query), SetParams);
            return rt.Uri;
        }

        
        


    }


    public enum PeriodType
    {
        Day,
        Week,
        Month,
        Year,
        Range
    }
}
