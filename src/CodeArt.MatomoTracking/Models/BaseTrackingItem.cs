using CodeArt.MatomoTracking.Attributes;
using CodeArt.MatomoTracking.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeArt.MatomoTracking.Models
{
    //https://developer.matomo.org/api-reference/tracking-api
    
    public abstract class BaseTrackingItem : ITrackingItem
    {
        /// <summary>
        /// The full URL for the current action.
        /// </summary>
        [QueryParameter("url")]
        public string Url { get; set; } //url

        /// <summary>
        /// The title of the action being tracked. It is possible to use slashes / to set one or several categories for this action. For example, Help / Feedback will create the Action Feedback in the category Help.
        /// </summary>
        [QueryParameter("action_name")]
        public string ActionName { get; set; } //action_name

        /// <summary>
        /// The unique visitor ID, must be a 16 characters hexadecimal string. Every unique visitor must be assigned a different ID and this ID must not change after it is assigned. If this value is not set Matomo (formerly Piwik) will still track visits, but the unique visitors metric might be less accurate.
        /// </summary>
        [QueryParameter("_id")]
        public string VisitorID { get; set; } //_id

        //User info:
        /// <summary>
        /// The full HTTP Referrer URL. This value is used to determine how someone got to your website (ie, through a website, search engine or campaign).
        /// </summary>
        [QueryParameter("urlref")]
        public string UrlRef { get; set; } //urlref

        /// <summary>
        /// The Campaign name. Used to populate the Referrers > Campaigns report. Note: this parameter will only be used for the first pageview of a visit.
        /// </summary>
        [QueryParameter("_rcn")]
        public string CampaignName { get; set; } //_rcn

        /// <summary>
        ///  The Campaign Keyword. Used to populate the Referrers > Campaigns report (clicking on a campaign loads all keywords for this campaign). Note: this parameter will only be used for the first pageview of a visit.
        /// </summary>
        [QueryParameter("_rck")]
        public string CampaignKeyword { get; set; } //_rck

        /// <summary>
        /// The resolution of the device the visitor is using, eg 1280x1024
        /// </summary>
        [QueryParameter("res")]
        public string Resolution { get; set; } //res, e.g. 1280x1024
        
        /// <summary>
        /// The user local time
        /// </summary>
        public TimeSpan? UserTime { get; set; } //h, m, s

        /// <summary>
        /// When set to true, the visitor's client is known to support cookies
        /// </summary>
        [QueryParameter("cookie")]
        public bool? UserSupportsCookies { get; set; } //cookie

        /// <summary>
        /// An override value for the User-Agent HTTP header field. The user agent is used to detect the operating system and browser used
        /// </summary>
        [QueryParameter("ua")]
        public string UserAgent { get; set; } //ua
        //public string UAData { get; set; } json doc

        /// <summary>
        /// An override value for the Accept-Language HTTP header field. This value is used to detect the visitor's country if GeoIP is not enabled.
        /// </summary>
        [QueryParameter("lang")]
        public string Language { get; set; } //lang

        /// <summary>
        ///  defines the User ID for this request. User ID is any non-empty unique string identifying the user (such as an email address or an username). To access this value, users must be logged-in in your system so you can fetch this user ID from your system, and pass it to Matomo. The User ID appears in the visits log, the Visitor profile, and you can Segment reports for one or several User ID (userId segment). When specified, the User ID will be "enforced". This means that if there is no recent visit with this User ID, a new one will be created. If a visit is found in the last 30 minutes with your specified User ID, then the new action will be recorded to this existing visit.
        /// </summary>
        [QueryParameter("uid")]
        public string UserID { get; set; } //uid - Users ID string for known users

        /// <summary>
        /// defines the visitor ID for this request. You must set this value to exactly a 16 character hexadecimal string (containing only characters 01234567890abcdefABCDEF). We recommended setting the User ID via uid rather than use this cid.
        /// </summary>
        [QueryParameter("CID")]
        public string CID { get; set; } //CID, 16 hex string for anonymous visitors

        /// <summary>
        /// If set to true, will force a new visit to be created for this action. 
        /// </summary>
        [QueryParameter("new_visit")]
        public bool? NewVisit { get; set; } //new_visit

        /// <summary>
        /// A Custom Dimension value for a specific Custom Dimension ID (requires Matomo 2.15.1 + Custom Dimensions plugin see the Custom Dimensions guide). If Custom Dimension ID is 2 use dimension[2]=dimensionValue to send a value for this dimension. The configured Custom Dimension has to be in scope "Visit" or "Action".
        /// </summary>
        public Dictionary<int, string> Dimensions { get; set; } = new Dictionary<int, string>(); //dimension1, dimension2, etc custom dimensions set to their value

        //-----------------------------------Optional Action info-----------------------------------------------

        /// <summary>
        /// An external URL the user has opened. Used for tracking outlink clicks. We recommend to also set the url parameter to this same value.
        /// </summary>
        [QueryParameter("link")]
        public string Link { get; set; } //link - External link opened by user

        /// <summary>
        /// URL of a file the user has downloaded. Used for tracking downloads. We recommend to also set the url parameter to this same value.
        /// </summary>
        [QueryParameter("download")]
        public string Download { get; set; } //download - URL of a file the user has downloaded

        /// <summary>
        /// The Site Search keyword. When specified, the request will not be tracked as a normal pageview but will instead be tracked as a Site Search request.
        /// </summary>
        [QueryParameter("search")]
        public string Search { get; set; } //search - Search query

        /// <summary>
        /// when search is specified, you can optionally specify a search category with this parameter.
        /// </summary>
        [QueryParameter("search_cat")]
        public string SearchCategory { get; set; } //search_cat

        /// <summary>
        /// when search is specified, we also recommend setting the search_count to the number of search results displayed on the results page. When keywords are tracked with &search_count=0 they will appear in the "No Result Search Keyword" report.
        /// </summary>
        [QueryParameter("search_count")]
        public int? SearchCount { get; set; } //search_count

        /// <summary>
        /// Accepts a six character unique ID that identifies which actions were performed on a specific page view. When a page was viewed, all following tracking requests (such as events) during that page view should use the same pageview ID. Once another page was viewed a new unique ID should be generated. Use [0-9a-Z] as possible characters for the unique ID
        /// </summary>
        [QueryParameter("pv_id")]
        public string PageViewID { get; set; } //pv_id If this action relates to a previous page view

        /// <summary>
        /// If specified, the tracking request will trigger a conversion for the goal of the website being tracked with this ID.
        /// </summary>
        [QueryParameter("idgoal")]
        public int? IDGoal { get; set; }//idgoal - will trigger a goal with that ID

        /// <summary>
        /// A monetary value that was generated as revenue by this goal conversion. Only used if idgoal is specified in the request.
        /// </summary>
        [QueryParameter("revenue")]
        public decimal? Revenue { get; set; } //revenue - Only use if idgoal is set. Monetary value of action

        /// <summary>
        /// The amount of time it took the server to generate this action, in milliseconds. This value is used to process the Page speed report Avg. generation time column in the Page URL and Page Title reports, as well as a site wide running average of the speed of your server.
        /// </summary>
        [QueryParameter("gt_ms")]
        public int? ServerTimeToGenerateActionInMS { get; set; } //gt_ms

        /// <summary>
        /// The charset of the page being tracked. Specify the charset if the data you send to Matomo is encoded in a different character set than the default utf-8.
        /// </summary>
        [QueryParameter("cs")]
        public string CharsetOfPageTracked { get; set; } //cs (default utf-8)

        /// <summary>
        /// Stands for custom action. &ca=1 can be optionally sent along any tracking request that isn't a page view. For example it can be sent together with an event tracking request e_a=Action&e_c=Category&ca=1. The advantage being that should you ever disable the event plugin, then the event tracking requests will be ignored vs if the parameter is not set, a page view would be tracked even though it isn't a page view. For more background information check out #16570. Do not use this parameter together with a ping=1 tracking request.
        /// </summary>
        [QueryParameter("ca")]
        public bool CustomAction { get; set; }//ca

        //-----------------------------------Other parameters (requires authentication) -----------------------------------------------

        /// <summary>
        /// If set, overrides value for the visitor IP (both IPv4 and IPv6 notations supported). Requires the AuthToken to be set in the options.
        /// </summary>
        [QueryParameter("cip")]
        public string OverrideVisitorIP { get; set; }//cip

        /// <summary>
        /// Override for the datetime of the request(normally the current time is used). Requires the AuthToken to be set in the options.
        /// </summary>
        [QueryParameter("cdt")]
        public DateTime? OverrideDateTime{ get; set; }

        /// <summary>
        /// Override value for the country. Should be set to the two letter country code of the visitor (lowercase). Requires the AuthToken to be set in the options.
        /// </summary>
        [QueryParameter("country")]
        public string OverrideCountry { get; set; }

        /// <summary>
        /// Override value for the region. Should be set to a ISO 3166-2 region code. Requires the AuthToken to be set in the options.
        /// </summary>
        [QueryParameter("region")]
        public string OverrideRegion { get; set; }

        /// <summary>
        /// An override value for the city the visitor is located in. Requires the AuthToken to be set in the options.
        /// </summary>
        [QueryParameter("city")]
        public string OverrideCity { get; set; }

        /// <summary>
        /// An override value for the visitors latitude. Requires the AuthToken to be set in the options.
        /// </summary>
        [QueryParameter("lat")]
        public decimal? OverrideLat { get; set; }

        /// <summary>
        /// An override value for the visitors longitude. Requires the AuthToken to be set in the options.
        /// </summary>
        [QueryParameter("long")]
        public decimal? OverrideLong { get; set; }
    }
}
