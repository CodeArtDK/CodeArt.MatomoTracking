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
        [QueryParameter("url")]
        public string Url { get; set; } //url

        [QueryParameter("action_name")]
        public string ActionName { get; set; } //action_name

        [QueryParameter("_id")]
        public string VisitorID { get; set; } //_id

        //User info:
        [QueryParameter("urlref")]
        public string UrlRef { get; set; } //urlref

        [QueryParameter("_rcn")]
        public string CampaignName { get; set; } //_rcn
        
        [QueryParameter("_rck")]
        public string CampaignKeyword { get; set; } //_rck

        [QueryParameter("res")]
        public string Resolution { get; set; } //res, e.g. 1280x1024
        
        public TimeSpan? UserTime { get; set; } //h, m, s
        
        [QueryParameter("cookie")]
        public bool? UserSupportsCookies { get; set; } //cookie

        [QueryParameter("ua")]
        public string UserAgent { get; set; } //ua
        //public string UAData { get; set; } json doc

        [QueryParameter("lang")]
        public string Language { get; set; } //lang

        [QueryParameter("uid")]
        public string UserID { get; set; } //uid - Users ID string for known users
        
        [QueryParameter("CID")]
        public string CID { get; set; } //CID, 16 hex string for anonymous visitors

        [QueryParameter("new_visit")]
        public bool? NewVisit { get; set; } //new_visit
        
        public Dictionary<int, string> Dimensions { get; set; } = new Dictionary<int, string>(); //dimension1, dimension2, etc custom dimensions set to their value

        //Optional Action info
        [QueryParameter("link")]
        public string Link { get; set; } //link - External link opened by user

        [QueryParameter("download")]
        public string Download { get; set; } //download - URL of a file the user has downloaded

        [QueryParameter("search")]
        public string Search { get; set; } //search - Search query
        [QueryParameter("search_cat")]
        public string SearchCategory { get; set; } //search_cat
        [QueryParameter("search_count")]
        public int? SearchCount { get; set; } //search_count
        [QueryParameter("pv_id")]
        public string PageViewID { get; set; } //pv_id If this action relates to a previous page view
        [QueryParameter("idgoal")]
        public int? IDGoal { get; set; }//idgoal - will trigger a goal with that ID
        public decimal Revenue { get; set; } //revenue - Only use if idgoal is set. Monetary value of action
        [QueryParameter("gt_ms")]
        public int? ServerTimeToGenerateActionInMS { get; set; } //gt_ms
        [QueryParameter("cs")]
        public string CharsetOfPageTracked { get; set; } //cs (default utf-8)
        [QueryParameter("ca")]
        public bool CustomAction { get; set; }//ca
    }
}
